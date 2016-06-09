app.factory('helper', function () {
    var self = {}
    self.isloggedin = function (obj) {
        return (obj.loggedIn === true) ? true : false;
    }
    self.isRegistering = function (obj) {
        return (obj.registering === true) ? true : false;
    }
    self.NumberOnly = function (model) {
        if (model !== undefined)
            return model.replace(/[^0-9]/g, '');
    }
    self.CharacterOnly = function (model) {
        if (model !== undefined)
            return model.replace(/[^a-zA-Z\s]*$/, '');
    }

    return self;
});


