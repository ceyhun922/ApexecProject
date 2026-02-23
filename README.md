# Apexec Web API

**Apexec** layihəsi üçün hazırlanmış .NET 6 əsaslı RESTful Web API. Xarici təhsil imkanları platformasının backend xidmətidir.

---

## Texnologiyalar

| Texnologiya | Versiya |
|---|---|
| .NET | 6.0 |
| Entity Framework Core | 6.0 |
| SQL Server | — |
| AutoMapper | 12.0 |
| FluentValidation | 11.3 |
| JWT Bearer Authentication | 6.0 |
| Serilog | 10.0 |
| Swagger (Swashbuckle) | 6.5 |
| BCrypt.Net | 4.1 |

---

## Arxitektura

- **Repository Pattern** — verilənlərə çıxışı izolə edir
- **Unit of Work** — tranzaksiyaları idarə edir
- **Service Layer** — biznes məntiqi controller-dən ayrılıb
- **Global Exception Middleware** — bütün xətalar mərkəzləşdirilmiş şəkildə idarə olunur
- **ApiResponse\<T\>** — bütün endpointlər standart wrapper cavab qaytarır
- **BaseEntity** — bütün entity-lər `Id`, `Status`, `CreatedDate` irsiyyət alır
- **Çoxdilli dəstək** — `az`, `en`, `ru`, `tr` dilləri dəstəklənir (URL route: `api/{lang}/...`)

---

## Modullar

| Modul | Route | Açıqlama |
|---|---|---|
| Auth | `api/auth` | Giriş, token yeniləmə, profil idarəetməsi |
| Heroes | `api/{lang}/heroes` | Əsas səhifə hero bölməsi |
| Abouts | `api/{lang}/abouts` | Haqqımızda bölməsi |
| Countries | `api/{lang}/countries` | Ölkələr |
| Education Levels | `api/{lang}/educationlevels` | Təhsil səviyyələri (ölkəyə görə filterlənir) |
| Departments | `api/{lang}/departments` | Şöbələr (təhsil səviyyəsinə görə filterlənir) |
| Summer Schools | `api/{lang}/summerschools` | Yay məktəbləri |
| Testimonials | `api/{lang}/testimonials` | Rəylər |
| FAQs | `api/{lang}/faqs` | Tez-tez verilən suallar |
| Search | `api/{lang}/search` | Ölkə → Təhsil səviyyəsi → Şöbə filterli axtarış |
| Contacts | `api/contacts` | Əlaqə məlumatları (sosial şəbəkə, telefon) |
| Messages | `api/messages` | Ziyarətçi mesajları |
| Informations | `api/informations` | Müraciət məlumatları |
| Footers | `api/footers` | Footer məlumatları |
| File Upload | `api/fileimage` | Şəkil yükləmə xidməti |

---

## Quraşdırma

### Tələblər

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server (local və ya uzaq)
- `dotnet-ef` aləti

```bash
dotnet tool install --global dotnet-ef --version 6.*
```

### Addımlar

```bash
# 1. Layihəni klon et
git clone <repo-url>
cd ApexWebAPI

# 2. Konfiqurasiya faylını yarat (appsettings.Development.json)
# aşağıdakı bölməyə bax

# 3. Migrasiyanı tətbiq et
dotnet ef database update

# 4. Layihəni işlat
dotnet run
```

---

## Konfiqurasiya

`appsettings.json` faylında aşağıdakı parametrləri tənzimləyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=ApexDb;User Id=...;Password=...;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "ən-azı-32-simvol-olan-gizli-açar",
    "Issuer": "api.apexec.az",
    "Audience": "admin.apexec.az",
    "ExpireMinutes": 60
  },
  "App": {
    "BaseUrl": "https://api.apexec.az"
  }
}
```

> **Qeyd:** JWT açarını mühit dəyişəni və ya `dotnet user-secrets` ilə saxlayın, heç vaxt koda yazmayın.

```bash
dotnet user-secrets set "Jwt:Key" "sizin-gizli-açarınız"
```

---

## Authentication

API **JWT Bearer Token** autentifikasiyasından istifadə edir.

```
POST api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "şifrə"
}
```

Cavab olaraq `token` alırsınız. Qorunan endpointlərə sorğu göndərərkən:

```
Authorization: Bearer <token>
```

---

## Fayl Yükləmə

Şəkillər `api/fileimage` endpointi vasitəsilə yüklənir:

```
POST api/fileimage
Content-Type: multipart/form-data

file: <şəkil faylı>
```

Cavab olaraq tam URL qaytarılır. Bu URL digər create/update endpointlərinin `ImageUrl` sahəsinə göndərilir.

Yüklənən fayllar `wwwroot/images/` qovluğunda saxlanılır.

---

## Axtarış

Kaskad dropdown axtarışı üçün:

```
GET api/{lang}/search?countryId=1&educationLevelId=2&departmentId=3&page=1&pageSize=10
```

- Yalnız `countryId` göndərilsə — o ölkəyə aid bütün şöbələr gəlir
- `educationLevelId` əlavə edilsə — daha da daraldılır
- Nəticə səhifələnmiş (`TotalCount`, `TotalPages`) şəkildə qaytarılır

---

## Loqlaşdırma

[Serilog](https://serilog.net/) ilə həm konsola, həm də fayla yazılır:

```
logs/log-YYYYMMDD.txt
```

---

## Swagger

Layihə işə düşdükdə Swagger UI avtomatik açılır:

```
https://localhost:{PORT}/swagger
```

---

## CI/CD

GitHub Actions ilə avtomatik deploy konfiqurasiya edilib (`.github/workflows/deploy.yml`):

1. `main` branch-ə push edildikdə tetiklenir
2. Kod serverdə `git pull` edilir
3. `dotnet publish` ilə build alınır
4. `dotnet ef database update` ilə migrasiya tətbiq edilir
5. `systemctl restart apex-api` ilə servis yenidən başladılır

Lazımi GitHub Secrets:

| Secret | Açıqlama |
|---|---|
| `SERVER_HOST` | Server IP ünvanı |
| `SSH_PRIVATE_KEY` | SSH giriş açarı |
| `PRODUCTION_DB_CONNECTION` | Production DB connection string |

---

## Layihə Strukturu

```
ApexWebAPI/
├── Controllers/        # API endpoint-ləri
├── DTOs/               # Data Transfer Object-lər
├── Entities/           # Verilənlər bazası modelləri
├── Mapping/            # AutoMapper profili
├── Middleware/         # Global xəta idarəetməsi
├── Migrations/         # EF Core migrasiiyaları
├── Repositories/       # Repository pattern tətbiqi
├── Services/           # Biznes məntiqi
├── ValidationRule/     # FluentValidation qaydaları
├── Common/             # ApiResponse, Pagination, LanguageCodes
├── Resources/          # Lokalizasiya faylları (.resx)
└── wwwroot/            # Statik fayllar (şəkillər, videolar)
```
