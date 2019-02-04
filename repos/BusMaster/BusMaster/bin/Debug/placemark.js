ymaps.ready(init);

function init() {
    

    var myMap = new ymaps.Map("map", {
        center: [55.14, 36.55],
        zoom: 10
    }, {
            searchControlProvider: 'yandex#search'
        }),
        myGeoObject = new ymaps.GeoObject({
            // Описание геометрии.
            geometry: {
                type: "Point",
                coordinates: [55.122038, 36.605472]
            },
            // Свойства.
            properties: {
                // Контент метки.
                iconContent: '100'               
            }
        }, {
                // Опции.
                // Иконка метки будет растягиваться под размер ее содержимого.
                preset: 'islands#blackStretchyIcon',
                iconColor: '#3b5998'
            })

    myMap.geoObjects
        .add(myGeoObject)
}
