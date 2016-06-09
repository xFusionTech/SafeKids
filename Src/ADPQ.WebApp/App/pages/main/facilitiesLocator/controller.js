app.config(['uiGmapGoogleMapApiProvider', function (GoogleMapApi) {
    GoogleMapApi.configure({
        key: 'AIzaSyBczL9Suhsir-Gr-8bLsv9hzVxmgbnOFFo',
        // v: '3.20',
        libraries: 'weather,geometry,visualization'
    });
}]);

app.controller('locatorController', ['$scope', "uiGmapObjectIterators", '$rootScope', '$http', function ($scope, uiGmapObjectIterators, $rootScope, $http) {
    $scope.zip = "";
    var lastId = 1;

    $scope.searchResults = {
        results: {
            length: 0
        },
        resultsTable: {
            length: 0
        },
        resultsAvailable : false
    };
    $scope.map = {
        doCluster: false,
        options: {
            streetViewControl: false,
            panControl: false,
            maxZoom: 18,
            minZoom: 4
        },
        events: {
            idle: function () {
                //$scope.searchResults.results = [];
                //$scope.addMarkers();
            }
        },
        zoom: 6,
        center: {
            latitude: 39,
            longitude: -122
        },
    };

    $scope.addMarkers = function () {
        $scope.searchResults.resultsTable = [];
        $scope.searchResults.resultsAvailable = false;
        var markers = {};
        var i = 0;

        $scope.url = "https://chhs.data.ca.gov/resource/mffa-c6z5.json";
        if ($scope.zip != "") {
            $scope.url += "?facility_zip=" + $scope.zip;
        }
        $http.get($scope.url).then(function (response) {
            //$http.get("https://chhs.data.ca.gov/resource/mffa-c6z5.json?facility_zip="+$scope.zip).then(function (response) {
            //$resource.get("https://chhs.data.ca.gov/resource/mffa-c6z5.json?$where=within_circle(location, 33.9697897, -118.2468148, 5000)").then(function(response) {
            $scope.rows = response.data;
            //console.log($scope.rows);
            for (i = 0; i < $scope.rows.length; i++) {
                if ($scope.rows[i].location != undefined) {
                    var cords = $scope.rows[i].location.coordinates;
                    markers[i] = {
                        'coords': {
                            'latitude': cords[1],
                            'longitude': cords[0]
                        },
                        'key': i
                    };
                    var result = {
                        "name": $scope.rows[i].facility_name,
                        "id": i,
                        "address": $scope.rows[i].facility_address,
                        "caseworker": $scope.rows[i].facility_administrator,
                        "phonenumber": $scope.rows[i].facility_telephone_number
                    }
                    $scope.searchResults.resultsTable.push(result);
                }
            }
            if ($scope.rows.length > 0)
            {
                $scope.searchResults.resultsAvailable = true;
            }
            $scope.searchResults.results = uiGmapObjectIterators.slapAll(markers);
            if ($scope.searchResults.results[0] != undefined)
            {
                $scope.map.center.latitude = $scope.searchResults.results[0].coords.latitude;
                $scope.map.center.longitude = $scope.searchResults.results[0].coords.longitude;
            }
            $scope.map.zoom = 14;
        });
    };

}]);