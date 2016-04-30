# CWI.PostManEvent

[![Build status](https://ci.appveyor.com/api/projects/status/2rwo1dewhvkwnnrx?svg=true)](https://ci.appveyor.com/project/GiovaniBarili/cwi-postmanevent)


PostManEvent é um framework para controle de eventos baseados em Publish e Subscribes de eventos organizado em Hubs!


Subscribe é a ação de inscrever-se ser acionaddo quando ouver um determinado tipo de evento.
Por exemplo, dado que quando seja alterado a inforamação de um usuário, o mesmo seja notificado. Teriamos um evento AlteracaoUsuarioEvent e um NotificacaoSubscribe. 
``` C#
hubEvent.Subscribe<AlteracaoUsuarioEvent, NotificacaoSubscribe>();
```
Nesse caso, sempre que ouver o Publish do evento AlteracaoUsuarioEvent, será acionado o NotificacaoSubscribe, e todos demais Subscribes associados e esse evento.


Os Hubs são formas de implementação de atendimento e controle dos Publishs.


