app.controller('loginController', ['$scope', 'service', '$rootScope', '$cookieStore', '$window', '$resource','helper', function ($scope, service, $rootScope, $cookieStore, $window, $resource,helper) {

    var self = Object.create(null);
    $cookieStore.put('session', { loggedIn: false, registering: false });
    self.submitted = false;
    self.rsubmitted = false;
    self.textRequired = false;
    self.emailRequired = true;
    self.defaultMethod = "email address";
    self.displayCodeSection = false;
    self.verificationCode = "";
	self.emailTaken = false;

    $rootScope.loginPage = true;
    $rootScope.loggedin = false;

    self.loginObj = {
        username: ($cookieStore.get('rememberme')) ? $cookieStore.get('rememberme') : '',
        password: '',
        rememberme: false
    };

    self.registerObj = {
        firstname: '',
        lastname: '',
        method: 'email',
        email: '',
        notifyEmail : '',
        text: '',
        terms: '',
        code: '',
    };
	
	self.checkEmail = function () {
        self.emailTaken = false;
		if(self.registerObj.email=="" || self.registerObj.email==undefined)
			self.registerObj.notifyEmail="";
		else
		{
			$resource("http://localhost:8090/api/Users", { id: self.registerObj.email, type:1 }).get(function (data) {
				if (data.result === true)
					self.emailTaken = true;
				else
					self.registerObj.notifyEmail = self.registerObj.email;
			});	
		}
    }

    self.authenticate = function () {
        if ($scope.loginform.$valid) {
            self.submitted = false;
            var login = {
                USERNAME: self.loginObj.username,
                PASSWORD: self.loginObj.password,
            };

            if (self.loginObj.rememberme)
                $cookieStore.put('rememberme', self.loginObj.username);
            else
                $cookieStore.remove("rememberme");

            $resource("http://localhost:8090/api/Users/Login").save(login, function (data) {
                if (data.TokenId != "00000000-0000-0000-0000-000000000000") {
                    $cookieStore.put('session', { loggedIn: true, registering: false });
                    $cookieStore.put('Userdata', data);
                    $window.location.href = "/App/index.html#/main";
                }
                else {
                    self.loginError = true;
                }
            });
        } else {
            self.submitted = true;
        }
    };

    self.notify = function (value) {
        self.registerObj.method = value;
        if (value === 'email') {
            self.textRequired = false;
            self.emailRequired = true;
            self.registerObj.text = '';
            self.defaultMethod = "email address";
        } else {
            self.emailRequired = false;
            self.textRequired = true;
            //self.registerObj.notifyEmail = '';
            self.defaultMethod = "mobile number";
        }
    }

    self.register = function () {
        if ($scope.registerform.$valid) {
            self.rsubmitted = false;
            $cookieStore.put('session', {loggedIn:true,registering:true});
            $cookieStore.put('registrationObj', self.registerObj);
            $window.location.href = "/App/index.html#/main";
        } else {
            self.rsubmitted = true;
        }
    }

    self.send = function (method, email, text) {
        var x = Math.floor(Math.random() * 10000);
        $cookieStore.put('Verification', x);
        self.displayCodeSection = true;
       // console.log('Verification Code : ' + x);
        var verification = {
            type: method,
            to: method=="email" ? email : text,
            code: x,
            name: self.registerObj.firstname
        };
        if (method == "email")
        {
            $resource("http://localhost:8088/api/CodeVerification").save(verification, function (data) {
            });
        }
        else if (method == "text")
        {
            $resource("http://localhost:8089/api/CodeVerification").save(verification, function (data) {
            });
        }
        //$resource("http://localhost:8085/api/CodeVerification").save(verification, function (data) {
        //});
    }

    self.codeVerified = false;

    self.verifyCode = function () {
        if ($cookieStore.get('Verification') == self.registerObj.code)
            self.codeVerified = true;
		else
			self.codeVerified = false;
    };


    return self;

}]);