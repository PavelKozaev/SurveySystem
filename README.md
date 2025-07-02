# SurveySystem

![.NET Core](https://img.shields.io/badge/.NET-9.0-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-blue)
![Docker](https://img.shields.io/badge/Docker-✔-blue)

**SurveySystem** — это веб-сервис для системы онлайн-опросов, реализованный на .NET 9.

## 🚀 Быстрый старт
Склонируйте репозиторий
```bash
git clone https://github.com/PavelKozaev/SurveySystem.git
```
Перейдите в корневую папку решения
```bash
cd SurveySystem
```
Выполните команду
```bash
docker-compose up -d --build
```

## Доступно по адресу:
- 🔗 http://localhost:8080 📚 Swagger UI

## API Endpoints
- Начать новое интервью для конкретного опроса
```bash
POST /api/surveys/{surveyId}/interviews
```
- Получить текущий вопрос для прохождения интервью
```bash
GET /api/interviews/{interviewId}/current-question
```
- Сохранить ответ на вопрос и получить ID следующего
```bash
POST /api/interviews/{interviewId}/results
```

## 🧪 
В базе данных уже создано 2 тестовых опроса. Для тестирования можно использовать ID первого опроса: 1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c.
- Выполните POST /api/surveys/1f9a3e42-1c7b-4b8f-8e4a-3e8a1d7f2b1c/interviews, чтобы начать опрос. Скопируйте interviewId из ответа.
- Выполните GET /api/interviews/{interviewId}/current-question, используя полученный interviewId, чтобы увидеть первый вопрос.
- Выполните POST /api/interviews/{interviewId}/results, чтобы ответить на вопрос.
- Повторяйте шаги 2 и 3, пока не получите ответ {"isCompleted": true}.

## Контрибьютинг

Если вы хотите внести свой вклад в проект, пожалуйста, создайте форк репозитория, создайте новую ветку, внесите свои изменения и отправьте pull request.

## Лицензия

Данный проект не лицензирован.

## Автор

[Pavel Kozaev](https://github.com/PavelKozaev)
