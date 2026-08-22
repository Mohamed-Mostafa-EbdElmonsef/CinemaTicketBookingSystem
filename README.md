# Cinema Ticket Booking System

مشروع ASP.NET Core MVC (net8.0) لحجز تذاكر السينما، يحتوي على:

- **واجهة عامة (Public site):** استعراض الأفلام، تفاصيل الفيلم، وحجز تذكرة.
- **لوحة تحكم (Admin Dashboard)** على المسار `/Admin`: إدارة الـ Category, Cinema, Movie, Actors, والحجوزات.

## الكيانات (Entities)
- **Category**: Id, Name
- **Cinema**: Id, Name, Address, Img
- **Actor**: Id, Name, Img
- **Movie**: Id, Name, Des, Price, Status, DateTime, MainImg, SubImages (صور متعددة), CategoryId, CinemaId, Actors (علاقة Many-to-Many)
- **Booking**: حجز تذكرة مرتبط بفيلم معيّن

## المتطلبات
- .NET 8 SDK
- SQL Server LocalDB (يأتي مع Visual Studio) أو أي SQL Server آخر

## خطوات التشغيل

1. فك ضغط المشروع وافتح الترمينال داخل المجلد.
2. استرجاع الحزم:
   ```bash
   dotnet restore
   ```
3. (اختياري) عدّل سلسلة الاتصال في `appsettings.json` إذا كنت تستخدم SQL Server غير LocalDB:
   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CinemaTicketBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   ```
4. إنشاء أول Migration (المشروع لا يحتوي على مجلد Migrations بعد لأن التوليد يحتاج dotnet-ef):
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate
   ```
5. تشغيل المشروع (سيقوم Program.cs تلقائياً بتطبيق الـ Migration وإضافة بيانات تجريبية أول مرة):
   ```bash
   dotnet run
   ```
6. افتح المتصفح على الرابط الذي يظهر في الطرفية (عادة `https://localhost:5001` أو ما شابه).
   - الموقع العام: `/`
   - لوحة التحكم: `/Admin`

## ملاحظات
- الصور المرفوعة (المدير يرفعها من لوحة التحكم) تُخزَّن في `wwwroot/uploads/{movies|cinemas|actors}`.
- لا يوجد نظام تسجيل دخول/صلاحيات في هذه النسخة الأولية؛ يمكن إضافة ASP.NET Core Identity لاحقاً لحماية مسار `/Admin`.
- تم استخدام Bootstrap 5 (RTL) عبر CDN، فلا حاجة لتثبيت أي حزم Frontend.
