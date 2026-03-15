# InQueue / SQA (Simple Queue Application)

**InQueue** — учебное приложение очередей на ASP.NET Core c Entity Framework. 

> В данном репозитории реализован минимальный кейс управления очередями, пользователями и ролями.

## 📁 Структура проекта

- `SQA.sln` — решение
- `SQA.Web/` — веб-приложение (ASP.NET Core MVC)
- `SQA.Domain/` — доменная модель, сущности, бизнес-исключения
- `SQA.EntityFramework/` — EF Core data layer, DbContext, модели и сервисы доступа
- `Integrated.Loggers/` — пользовательский логгер
- `SQA.Test/` — модульные тесты

## 🚀 Что реализовано

- CRUD для очередей, пользователей, ролей
- Хранение данных в SQLite через EF Core
- Средства аутентификации/авторизации и хеширования паролей
- Настраиваемый логгер
- Паттерн репозиториев/сервисов и разделение слоев

## ⚙️ Быстрый старт (Windows)

1. Откройте решение `SQA.sln` в Visual Studio или VS Code.
2. Убедитесь, что у вас установлен .NET 8 (или версия, указанная в проектах).
3. В корне репозитория выполните:

```powershell
cd InQueue
dotnet build SQA.sln
```

4. Запустите веб-проект:

```powershell
dotnet run --project SQA.Web\SQA.Web.csproj
```

5. Откройте браузер на `https://localhost:5001` (или URL, который покажет приложение).

## 🧪 Запуск тестов

```powershell
dotnet test SQA.Test\sqa.test.csproj
```

## 🔧 Конфигурация

- `SQA.Web/appsettings.json` содержит настройки приложения.
- `SQA.Web/DbPath.txt` указывает файл SQLite.
- В `SQA.EntityFramework/SQADbContextFactory.cs` настроено создание DbContext.

## 📌 Как вносить изменения

1. Измените доменные сущности в `SQA.Domain`.
2. Обновите слой данных и миграции в `SQA.EntityFramework` (если требуется).
3. Подключите сервисы и контроллеры в `SQA.Web`.
4. Запустите тесты и проверяйте бизнес-логику.

## 📝 Полезные команды

- `dotnet clean SQA.sln`
- `dotnet build SQA.sln`
- `dotnet run --project SQA.Web\SQA.Web.csproj`
- `dotnet test SQA.Test\sqa.test.csproj`
