using MongoDB.Driver;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.DataBase.Mongodb;
using quan_ly_kho.DataBase.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quan_ly_kho.common.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password, string userId = null);
        IEnumerable<User> GetAll();
        User GetById(string id);
        User GetByName(string username);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(string id);
    }

    public class UserService : IUserService
    {
        private MongoDBContext _context;

        public UserService(MongoDBContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password, string userId = null)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
                return null;

            //1 duyet
            //3 dang ky chua duyet
            var user = _context.sys_user_col.AsQueryable().Where(d => d.status_del != 0 && d.status_del != 2).SingleOrDefault(x => x.Username == username || x.id == userId);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.sys_user_col.AsQueryable();
        }

        public User GetById(string id)
        {
            return _context.sys_user_col.AsQueryable().Where(q => q.id == id).FirstOrDefault();
        }
        public User GetByName(string username)
        {
            return _context.sys_user_col.AsQueryable().Where(d => d.Username == username).FirstOrDefault();
        }


        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.sys_user_col.AsQueryable().Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.id = Guid.NewGuid().ToString();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.sys_user_col.InsertOneAsync(user);

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.sys_user_col.AsQueryable().Where(q => q.id == userParam.id).FirstOrDefault();

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.sys_user_col.AsQueryable().Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            // update user properties if provided
            //if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            //    user.FirstName = userParam.FirstName;

            //if (!string.IsNullOrWhiteSpace(userParam.LastName))
            //    user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                var update_pass = Builders<User>.Update

               .Set(x => x.PasswordHash, user.PasswordHash)
               .Set(x => x.PasswordSalt, user.PasswordSalt)
               ;

                // Create a filter to match the document to update
                var filter_pass = Builders<User>.Filter.Eq(x => x.id, userParam.id);
                _context.sys_user_col.UpdateOne(filter_pass, update_pass);

            }

            //  var update_name = Builders<User>.Update
            //.Set(x => x.FirstName, user.FirstName)
            //.Set(x => x.LastName, user.LastName)
            //;

            // // Create a filter to match the document to update
            // var filter_name = Builders<User>.Filter.Eq(x => x.id, userParam.id);
            // _context.sys_user_col.UpdateOne(filter_name, update_name);



        }

        public void Delete(string id)
        {


            var filter = Builders<User>.Filter.Eq(q => q.id, id);

            _context.sys_user_col.DeleteOne(filter);

        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}