# Panduan

---

## 1. Spesifikasi Lingkungan Sistem
- **Sistem Operasi**: macOS Monterey (12.0) atau lebih baru (Apple Silicon M-Series maupun Intel).
- **Runtime & SDK**: .NET 8.0 SDK (LTS).
- **Database**: PostgreSQL 16 / 15 / 14.
- **Sistem Kontrol Versi**: Git (untuk sinkronisasi repositori GitHub).
- **Code Editor**: Visual Studio Code atau JetBrains Rider.

---

## 2. Instalasi Package & Dependency via Terminal (Homebrew)

### Instalasi Homebrew (Package Manager macOS)
```bash
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```

### Instalasi Git
```bash
brew install git
```

### Instalasi .NET 8.0 SDK
```bash
brew install --cask dotnet-sdk
```

### Instalasi Database PostgreSQL 16 & Memulai Layanan
```bash
brew install postgresql@16
brew services start postgresql@16
```

### Instalasi Visual Studio Code
```bash
brew install --cask visual-studio-code
```

---

## 3. Instalasi Package & Dependency via Link Web Resmi (Installer Grafis `.pkg` / `.dmg`)

### Git Installer
- Unduh installer Git untuk macOS dari situs resmi Git: [https://git-scm.com/download/mac](https://git-scm.com/download/mac)
- Atau instal via Apple Command Line Tools dengan menjalankan perintah `xcode-select --install` di Terminal.

### .NET 8.0 SDK Installer
- Kunjungi tautan resmi Microsoft .NET: [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Pilih **macOS Arm64** (Apple Silicon M1/M2/M3) atau **macOS x64** (Prosesor Intel).
- Unduh berkas `.pkg` dan jalankan instalasi hingga selesai.

### PostgreSQL Database Server
- Kunjungi tautan resmi Postgres.app: [https://postgresapp.com](https://postgresapp.com)
- Unduh **Postgres.app dengan PostgreSQL 16**, pindahkan ke folder *Applications*, lalu klik tombol **Initialize**.
- (Opsional) Manajemen visual database dapat menggunakan **DBeaver**: [https://dbeaver.io/download/](https://dbeaver.io/download/) atau **pgAdmin 4**: [https://www.pgadmin.org/download/](https://www.pgadmin.org/download/)

### Visual Studio Code
- Kunjungi tautan resmi VS Code: [https://code.visualstudio.com/Download](https://code.visualstudio.com/Download)
- Unduh versi **Mac Universal**, pindahkan ke folder *Applications*.

---

## 4. Langkah-Langkah Menjalankan Sistem

### Navigasi ke Direktori Proyek
```bash
cd ~/Documents/OT
```

### Verifikasi Versi .NET SDK & Git
```bash
git --version
dotnet --version
```
*(Pastikan keluaran menunjukkan versi Git dan `8.0.xxx` untuk .NET).*

### Memulihkan Dependency Pustaka NuGet (Restore)
```bash
dotnet restore
```

### Instalasi Entity Framework Core CLI (Global Tool)
```bash
dotnet tool install --global dotnet-ef
```

### Konfigurasi Koneksi Database (`appsettings.json`)
Sesuaikan parameter koneksi PostgreSQL pada berkas `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=OT_Commerce;Username=postgres;Password=password_postgres"
  }
}
```

### Eksekusi Migrasi & Pembuatan Tabel Database
```bash
dotnet ef database update
```

### Kompilasi & Menjalankan Server Aplikasi
```bash
dotnet run
```
Server lokal akan aktif dan dapat diakses melalui browser pada alamat:
- **http://localhost:5000** (atau URL alternatif yang tertera di Terminal).

---

## 5. Kendala Error, Diagnosa & Solusi Terbaik (Troubleshooting)

### Kendala 1: Error `Failed to bind to address http://127.0.0.1:5000: address already in use`
- **Penyebab**: Port 5000 di macOS Monterey ke atas secara default digunakan oleh layanan **AirPlay Receiver**, atau terdapat proses `dotnet` lain yang masih berjalan di latar belakang.
- **Solusi 1 (Mematikan AirPlay Receiver)**:
  Buka *System Settings (Pengaturan Sistem)* -> *General (Umum)* -> *AirDrop & Handoff* -> Nonaktifkan **AirPlay Receiver**.
- **Solusi 2 (Menghentikan Proses pada Port 5000)**:
  Jalankan perintah berikut untuk mencari PID dan menghentikan proses yang menempati port 5000:
  ```bash
  lsof -i :5000
  kill -9 <PID_PROSES>
  ```
- **Solusi 3 (Menjalankan pada Port Alternatif)**:
  ```bash
  dotnet run --urls "http://localhost:8080"
  ```

### Kendala 2: Error `Connection refused` atau `Npgsql.NpgsqlException: Failed to connect to [localhost:5432]`
- **Penyebab**: Layanan PostgreSQL belum berjalan atau port 5432 tidak aktif.
- **Solusi via Homebrew**:
  ```bash
  brew services restart postgresql@16
  ```
- **Solusi via Postgres.app**:
  Buka aplikasi **Postgres.app** dari folder *Applications* dan pastikan server dalam status **Running (Green Checkmark)**. Cek status koneksi melalui terminal:
  ```bash
  pg_isready -h localhost -p 5432
  ```

### Kendala 3: Error `command not found: dotnet` setelah Instalasi
- **Penyebab**: Path .NET belum terdaftar di *environment variables* shell (`zsh`).
- **Solusi**:
  Tambahkan path .NET ke dalam konfigurasi `~/.zshrc`:
  ```bash
  echo 'export PATH="$PATH:/usr/local/share/dotnet:~/.dotnet/tools"' >> ~/.zshrc
  source ~/.zshrc
  ```

### Kendala 4: Error `dotnet-ef : command not found` saat Migrasi Database
- **Penyebab**: Alat global CLI `dotnet-ef` belum terinstal atau folder `~/.dotnet/tools` belum masuk ke dalam PATH.
- **Solusi**:
  ```bash
  dotnet tool update --global dotnet-ef
  export PATH="$PATH:$HOME/.dotnet/tools"
  dotnet ef database update
  ```

### Kendala 5: Error `xcrun: error: invalid active developer path` saat Menjalankan `git`
- **Penyebab**: Apple Command Line Tools hilang atau memerlukan pembaruan setelah pembaruan sistem operasi macOS.
- **Solusi**:
  ```bash
  xcode-select --install
  ```

### Kendala 6: Error `Permission denied` atau `Operation not permitted`
- **Penyebab**: Proteksi keamanan Gatekeeper macOS atau hak akses sistem berkas pada folder proyek yang diunduh dari internet.
- **Solusi**:
  Hapus flag karantina Gatekeeper dan berikan izin baca/tulis penuh pada folder proyek:
  ```bash
  xattr -cr ~/Documents/OT
  chmod -R 755 ~/Documents/OT
  ```
