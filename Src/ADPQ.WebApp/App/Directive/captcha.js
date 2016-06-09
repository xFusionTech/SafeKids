app.directive('captcha', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var captcha = new CAPTCHA(scope.$eval(attrs.captcha));
            $(element).on('click', function () {
                generate();
            });
            function generate() {
                captcha.generate();
            }
            generate();
        }
    };
});