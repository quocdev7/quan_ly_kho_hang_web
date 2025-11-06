# ===================================================================================
# Deploy Angular to R2 - Standard Sync Method (Recommended)
# ===================================================================================

# ---- Cấu hình Dự án ----
$remoteName   = "cloudflare_r2" # Tên remote trong file rclone.conf
$bucket       = "aisava-angular-cdn"
$distPath     = "$PSScriptRoot\dist"

# ---- Cấu hình Rclone ----
$configPath = "$PSScriptRoot\rclone.conf"
$remotePath = "$($remoteName):$($bucket)"

# Kiểm tra xem file config có tồn tại không
if (-not (Test-Path $configPath)) {
    Write-Host "Lỗi: Không tìm thấy tệp rclone.conf tại '$configPath'." -ForegroundColor Red
    exit 1
}

Write-Host "Starting deployment of Angular to R2 using config: $configPath"

# ==== Bước 1: Build Angular (nếu cần) ====
# Đảm bảo bạn đã build phiên bản mới nhất của ứng dụng
Write-Host "Building Angular for production..."
ng build --configuration production

# ==== Bước 2: Đồng bộ hóa toàn bộ thư mục dist lên R2 ====
# Lệnh 'sync' sẽ tự động tải lên file mới và xóa các file build cũ không còn dùng đến.
Write-Host "Syncing '$distPath' to R2 bucket '$bucket'..."
rclone sync $distPath $remotePath --config $configPath --progress --verbose

# Kiểm tra xem sync có thành công không
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error during sync operation. Deployment failed." -ForegroundColor Red
    exit 1
}

Write-Host "Deployment completed successfully! R2 bucket is now an exact mirror of the dist folder."