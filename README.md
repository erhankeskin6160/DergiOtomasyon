# 📰 Dergi Otomasyon Sistemi

Bu proje, ASP.NET Core MVC ile geliştirilmiş, kullanıcıların dergi aboneliklerini yönetebileceği, ödünç alma, abonelik yenileme, gecikme cezaları ve hatırlatma e-postalarını içeren tam kapsamlı bir otomasyon sistemidir. Rol bazlı yetkilendirme, filtreleme, arka plan servisleri ve kullanıcı dostu arayüz ile birlikte gerçek dünya senaryolarını desteklemektedir.

---

## 🚀 Temel Özellikler

### 👤 Kullanıcı Paneli
- 📚 Dergi listesi görüntüleme
- 🔍 Filtreleme (kategoriye, tarihe, isme göre)
- 📥 Dergi ödünç alma / iade işlemleri
- 🧾 Kendi abonelik geçmişini görüntüleme
- 🔐 Şifremi unuttum ve parola sıfırlama

### 🛠️ Admin Paneli
- ➕ Dergi ekleme / silme / güncelleme
- 👥 Kullanıcı yönetimi
- 📈 Aktif abonelikleri ve cezalıları listeleme
- 🗂 Kategoriler, yazarlar ve yayın evleri CRUD
- 📊 Filtreleme ve sıralama (tarih, kullanıcı, kategori vb.)

### 🔄 Arka Plan Servisleri
- 📧 **Hatırlatma Servisi**  
  Bitmek üzere olan abonelikler için kullanıcıya otomatik mail atar.

- ♻️ **Yenileme Servisi**  
  Vadesi gelen abonelikleri sistem otomatik olarak yeniler.

- ❗ **Ceza Servisi**  
  Süresi dolmuş ve yenilenmemiş aboneliklere otomatik ceza tanımlar.

### ✉️ E-Posta Bildirimi
- MailKit kullanılarak SMTP üzerinden çalışan bildirim sistemi
- Şifre sıfırlama, abonelik yenileme, ceza uyarısı gibi bildirimler

---

## 🧰 Kullanılan Teknolojiler

| Teknoloji             | Açıklama                                  |
|----------------------|-------------------------------------------|
| ASP.NET Core MVC     | Uygulama çatısı                           |
| Entity Framework Core| ORM ve veri işlemleri                     |
| Identity             | Giriş / Kayıt / Rol bazlı yetki sistemi   |
| BackgroundService    | Zamanlayıcılı arka plan servisleri        |
| MailKit              | SMTP ile e-posta gönderimi                |
| LINQ                 | Gelişmiş veri filtreleme ve sorgular      |
| SQL Server           | Veritabanı sistemi                        |
| Bootstrap            | Kullanıcı dostu responsive tasarım        |

---

## 🧠 Servis Yapısı ve İş Mantığı

| Servis Adı                         | Açıklama |
|-----------------------------------|----------|
| `SubscriptionService.cs`          | Abonelik CRUD ve yönetimi |
| `EmailService.cs`                 | SMTP ile e-posta gönderimi |
| `AutoPenaltyService.cs`           | Ceza hesaplama ve kayıt işlemleri |
| `SubscriptionRenewalService.cs`   | Vadesi gelen abonelikleri yeniler |
| `SubscriptionReminderService.cs`  | Kullanıcılara süre bitmeden uyarı yollar |
| `...BackgroundService.cs` dosyaları | İlgili servisleri arka planda periyodik çalıştırır |
