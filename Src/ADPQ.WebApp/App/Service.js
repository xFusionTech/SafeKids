

app.service("service", function ($resource) {
    this.login = function () {
        alert("Hello");
        return $resource("/Account/UserLogin");
    }
    
});
