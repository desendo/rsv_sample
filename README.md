# rsv_sample
пример архитектуры Rule-Service-View
точка входа - класс Di. игру запускает тот, кто первый его вызывал (обычно это view). на очередность выполнения это не влияет

взаимодействие происходит через реактивные переменные и signal bus как обратную связь со слоя View.

игра представляет собой состояние (обратите внимание на систему загрузки и сохранения)

игра стартует из изначального состояния сериализованном в ассете DefaultStateDataContainer
