# Технологии разработки приложений на базе фреймворков. Задание № 1.
## Выполнил: Еремин Кирилл Денисович, ЭФБО-10-23

### Запуск
```bash
dotnet run
```
Сервер запустится на `http://localhost:5000`.

### Доступные endpoints

| Метод | URL                  | Описание                          |
|-------|----------------------|-----------------------------------|
| GET   | `/`                  | Информация об API                 |
| GET   | `/api/materials`     | Список всех материалов            |
| GET   | `/api/materials/{id}`| Материал по ID                    |
| POST  | `/api/materials`     | Создать новый материал            |
|DELETE | `/api/materials/{id}`| Удалить материал по ID            |

### Примеры запросов

### Получить все материалы
```bash
curl http://localhost:5000/api/materials
```

### Создать материал
```bash
curl -X POST http://localhost:5000/api/materials \
  -H "Content-Type: application/json" \
  -d '{"name":"Кирпич","unitOfMeasure":"pcs","pricePerUnit":25,"quantityInStock":1000}'
```

### Получить материал по ID
```bash
curl http://localhost:5000/api/materials/{id}
```

### Удалить материал по ID
```bash
curl -X DELETE http://localhost:5000/api/materials/{id}
```

## Формат ошибок
Все ошибки возвращаются в едином формате:
```json
{
  "errorCode": "VALIDATION_ERROR",
  "message": "Name is required.",
  "requestId": "a1b2c3d4-...",
  "timestamp": "2025-02-23T12:00:00Z"
}
```