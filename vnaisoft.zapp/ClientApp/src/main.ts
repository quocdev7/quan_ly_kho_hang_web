import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { environment } from 'environments/environment';
import { AppModule } from 'app/app.module';
import { registerLicense } from '@syncfusion/ej2-base';

if ( environment.production )
{
    enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
                        .catch(err => console.error(err));
                        
registerLicense("MzgwNTUyNUAzMjMwMmUzNDJlMzBPblBWbXdKY1I1ZjBGajhRRjdQZ3RsNkh3L3d4ZTZnSlFTSEZHc2Rnem5nPQ==;MzgwNTUyNkAzMjMwMmUzNDJlMzBYTXV6cS9uUVh5bGU3cXRqS2JYMlVMRy9xMFpTbjNiRmxOUG41UGRqMUlrPQ==;Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQFhjTX5adkVmW35Yc3FXQGteUQ==;Mgo+DSMBPh8sVXJ0S0J+XE9HflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3tTckVqWX9bcnVTRWZUV090Xw==;ORg4AjUWIQA/Gnt2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxWdkFjUH9edXZUQGNVWEx9XUI=;NRAiBiAaIQQuGjN/V0Z+WE9EaFxKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35Rc0VnWXZecHFXR2ZYUEFyVEBd;MzgwNTUzMUAzMjMwMmUzNDJlMzBTSklad1cxMHNXeVdHcGJSTTg0aGJmZjdCSEtCSlJMRlZRbWdNMFNDbEZNPQ==;MzgwNTUzMkAzMjMwMmUzNDJlMzBEOGU3Z1hLY0tvRkowZGh0eGJzYmxCRld4c1VKTTlNemxFVWRSRGFvL3NJPQ==;Mgo+DSMBMAY9C3t2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxWdkFjUH9edXZUQGVdUU19XUI=;MzgwNTUzNEAzMjMwMmUzNDJlMzBibGVKeUZydFppVUlMRjJNT3dDaDJ6dXVJWGVTSktHSmhFR0I3akFEVnlvPQ==;MzgwNTUzNUAzMjMwMmUzNDJlMzBiQnRoYzhyM2J1dFo4MUdHY3RRZG8rdTVQOWV2VEtiUGlybjV2aWdNdUlBPQ==;MzgwNTUzNkAzMjMwMmUzNDJlMzBTSklad1cxMHNXeVdHcGJSTTg0aGJmZjdCSEtCSlJMRlZRbWdNMFNDbEZNPQ==");
