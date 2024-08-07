## Dahua.Server

Простое программное решение для обработки события фиксации человека на камере видеонаблюдения Dahua.

Использует SDK ```General_NetSDK_Eng_CSharp_Win64_IS_V3.057.0000003.0.R.240115``` (https://depp.dahuasecurity.com/integration/guide/download/sdk)

Подключение к камере осуществляется по IP устройства, логину и паролю.

При обнаружении человека, срабатывает событие, из которого формируется сообщение и рассылается клиентам через SignalR.

Имя hub-а для прослушки - ```event```.

### Логирование

Почти все логи пишутся в уровень ```Trace```, по умолчанию вывод логов стоит на уровень ```Information```. Поменять можно в ```appsettings.json```.

## Запуск

Для запуска сервиса, необходимо добавить в ```appsettings.json``` следующий раздел:

```
  "Dahua": {
    "Ip": "XX.XXX.X.XX",
    "Port": XXXX,
    "Username": "user",
    "Password": "password"
  },
```

## Тестирование

Для тестирование можно использовать ```Postman```.
1. Открыть подключение по WebSocket;
2. Указать адрес прослушки (по умолчанию ```ws://localhost:5009/event```) и подключиться (статус должен поменяться на ```Connected```);
3. Отправить сообщение для рукопожатия (```{"protocol":"json","version":1}```), где указывается способ общения.

После выполния вышеперечисленных шагов, на клиент будут приходить сообщения - пинг и обнаружение человека.