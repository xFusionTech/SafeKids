app.directive("limitTo", [function () {
    return {
        restrict: "A",
        link: function (scope, elem, attrs) {
            angular.element(elem).on("keyup", function (e) {
                var limit = parseInt(attrs.limitTo);
                var nextElement = attrs.nextElement;
                if (this.value.length == limit)
                {
                    e.preventDefault();
                    if (nextElement !== undefined)
                        document.getElementById(nextElement).focus();
                }
                else if (this.value.length > limit)
                {
                    this.value = this.value.substring(0, limit);
                }
            });
        }
    }
}]);