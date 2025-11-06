
$remoteName   = "cloudflare_r2"

# Ten bucket R2 cua ban
$bucket       = "front-end-web"

# Ten thu muc tren bucket R2 ma ban muon dong bo file vao
$targetFolder = "schooltest"

# Domain public cua CDN (KHONG co dau / o cuoi).
# Vi du: "https://pub-xxxxxxxx.r2.dev" hoac "https://cdn.yourdomain.com"
$cdnDomain    = "https://cdnfrontend.vietnamai.com.vn" # <-- QUAN TRONG: THAY BANG DOMAIN CUA BAN

# Base Href cho ung dung. Thuong la "/" neu chay o goc domain.
$baseHref     = "/"

#----------------------------------------------------------------------------------
#---- KET THUC PHAN CAU HINH - KHONG CAN SUA CODE BEN DUOI ----
#----------------------------------------------------------------------------------

# Tu dong tao cac duong dan can thiet tu cau hinh ben tren
$distPath     = Join-Path -Path $PSScriptRoot -ChildPath "dist"
$configPath   = Join-Path -Path $PSScriptRoot -ChildPath "rclone.conf"
$deployUrl    = "$($cdnDomain)/$($targetFolder)/"
$remotePath   = "$($remoteName):$($bucket)/$($targetFolder)"


#---- BAT DAU QUA TRINH DEPLOY ----

# Kiem tra su ton tai cua file cau hinh rclone
if (-not (Test-Path -Path $configPath -PathType Leaf)) {
    # Su dung tieng Viet khong dau de tranh loi encoding
    Write-Host "LOI: Khong tim thay file 'rclone.conf' tai duong dan '$configPath'." -ForegroundColor Red
    Write-Host "Vui long dam bao file rclone.conf nam cung thu muc voi script nay." -ForegroundColor Red
    # Dung script neu khong tim thay file config
    exit 1
}

Write-Host "Bat dau deploy Angular len R2..." -ForegroundColor Cyan
Write-Host "  - Thu muc nguon : $distPath"
Write-Host "  - Dich R2       : $remotePath"
Write-Host "  - Base Href     : $baseHref"
Write-Host "  - Deploy URL    : $deployUrl"
Write-Host "  - Config        : $configPath"

#==== Buoc 1: Build ung dung Angular voi cau hinh CDN ====
Write-Host ""
Write-Host "==== Buoc 1: Dang build ung dung Angular cho production... ====" -ForegroundColor Yellow

# Them tham so --base-href va --deploy-url vao lenh build
ng build --configuration production --base-href $baseHref --deploy-url $deployUrl

# Kiem tra ket qua build
if ($LASTEXITCODE -ne 0) {
    Write-Host "LOI: Qua trinh build Angular that bai. Vui long kiem tra lai loi va thu lai." -ForegroundColor Red
    exit 1
}

#==== Buoc 2: Dong bo hoa len R2 ====
Write-Host ""
Write-Host "==== Buoc 2: Dang dong bo hoa thu muc '$distPath' len '$remotePath'... ====" -ForegroundColor Yellow

# Su dung rclone sync de day code len R2
rclone sync $distPath $remotePath --config $configPath --progress --verbose

# Kiem tra ket qua cua lenh rclone
if ($LASTEXITCODE -ne 0) {
    Write-Host "LOI: Qua trinh dong bo voi rclone that bai. Deploy khong thanh cong." -ForegroundColor Red
    exit 1
}

#==== THONG BAO HOAN TAT ====
Write-Host ""
Write-Host "----------------------------------------------------------------" -ForegroundColor Green
Write-Host "  DEPLOY THANH CONG!" -ForegroundColor Green
Write-Host "  Thu muc '$targetFolder' tren bucket '$bucket' da duoc cap nhat." -ForegroundColor Green
Write-Host "  Ung dung da duoc cau hinh de tai tai nguyen tu: $deployUrl" -ForegroundColor Green
Write-Host "----------------------------------------------------------------"