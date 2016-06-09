app.config(['$routeProvider',
  function ($routeProvider) {
      var path = '/app/pages';
      $routeProvider.
        when('/', {
            templateUrl: path + '/login/index.html',
            controller: 'loginController',
        }).
        when('/main', {
            templateUrl: path + '/main/index.html',
            controller: 'mainController',
        })
  }]);
