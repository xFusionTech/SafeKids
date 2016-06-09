app.controller('parentProfileController', ['$scope', 'service', '$rootScope', '$cookieStore', 'helper', '$resource', '$window', function ($scope, service, $rootScope, $cookieStore, helper, $resource, $window) {

    var self = Object.create(null);

    self.helper = helper;
    self.token = '';
    self.PERS_ID = '';
    self.emailRequired = true;
    self.usernameTaken = false;
    self.enableEdit = true;
    self.ParentProfileList = [];
    self.changePassword = false;
    self.hideChangePassword = false;


    $scope.$on('refreshParentProfilePhoto', function (event, args) {
       
        $scope.$apply(function () {
            self.ParentProfileList.push({ name: args.name, image: args.Image });         
        });
    });


    self.isRegistering = helper.isRegistering($cookieStore.get('session'));

    self.validationError = false;

    self.hziplast = false;
    self.mziplast = false;

    self.zipExtCheck = function (section, model) {
        if (model === undefined)
            return;
        if(section === 'home')
        {
            self.hziplast = (model.length > 0 && model.length !== 4) ? true : false;
        }
        else
        {
            self.mziplast = (model.length > 0 && model.length !== 4) ? true : false;
        }
    };

    self.getParent = function (PERS_ID, TokenId) {

        $resource("http://localhost:8085/api/Register/:token/:id", { id: PERS_ID, token: TokenId }).get(function (data) {
            self.showMailing((data.result.MAILING_ADR === true) ? 'same' : 'other');
            var mobile = '';
            var work = '';
            var home = '';
            angular.forEach(data.result.PhoneTypes, function (value, key) {
                if (value.PHON_TYPE_CD === 'mobile')
                    mobile = value.PHON_TYP_DESC;
                if (value.PHON_TYPE_CD === 'work')
                    work = value.PHON_TYP_DESC;
                if (value.PHON_TYPE_CD === 'home')
                    home = value.PHON_TYP_DESC;
            });
            self.ParentProfileList = [];
            angular.forEach(data.result.PersonProfilePic, function (value, key) {
                self.ParentProfileList.push({ name: value.DIS_PIC_ID, image: value.image });
            });
            var homeAddress = {};
            var mailingAddress = {};
            angular.forEach(data.result.Address, function (value, key) {
                if (value.ADR_DESC === 'home') {
                    homeAddress.ADR_LINE1 = value.ADR_LINE1;
                    homeAddress.ADR_LINE2 = value.ADR_LINE2;
                    homeAddress.ADR_CITY = value.ADR_CITY;
                    homeAddress.ADR_STATE_CD = value.ADR_STATE_CD;
                    homeAddress.ADR_ZIP_CD = value.ADR_ZIP_CD;
                    homeAddress.ADR_ZIP_EXTN = (value.ADR_ZIP_EXTN === 0) ? '' : value.ADR_ZIP_EXTN;
                }
                if (value.ADR_DESC === 'mailing') {
                    mailingAddress.ADR_LINE1 = value.ADR_LINE1;
                    mailingAddress.ADR_LINE2 = value.ADR_LINE2;
                    mailingAddress.ADR_CITY = value.ADR_CITY;
                    mailingAddress.ADR_STATE_CD = value.ADR_STATE_CD;
                    mailingAddress.ADR_ZIP_CD = value.ADR_ZIP_CD;
                    mailingAddress.ADR_ZIP_EXTN = (value.ADR_ZIP_EXTN === 0) ? '' : value.ADR_ZIP_EXTN;
                }
            });

            self.parentProfile = {
                parent: {
                    firstname: data.result.personname.PERS_FNAME,
                    middlename: data.result.personname.PERS_MNAME,
                    lastname: data.result.personname.PERS_LNAME,
                    gender: data.result.Person.Gender,
                    social: {
                        first: data.result.Person.Social_Security_No.substring(0, 3),
                        middle: data.result.Person.Social_Security_No.substring(3, 5),
                        last: data.result.Person.Social_Security_No.substring(5, 9)
                    },
                },
                contact: {
                    home: {
                        first: home.substring(0, 3),
                        middle: home.substring(3, 6),
                        last: home.substring(6, 10),
                    },
                    work: {
                        first: work.substring(0, 3),
                        middle: work.substring(3, 6),
                        last: work.substring(6, 10),
                    },
                    mobile: {
                        first: mobile.substring(0, 3),
                        middle: mobile.substring(3, 6),
                        last: mobile.substring(6, 10),
                    },
                    email: data.result.Person.email,
                    bestTimeToCall: data.result.Person.TIMETOCALL.toString(),
                    address: {
                        homeAddress: {
                            street1: homeAddress.ADR_LINE1,
                            street2: homeAddress.ADR_LINE2,
                            city: homeAddress.ADR_CITY,
                            state: homeAddress.ADR_STATE_CD,
                            zip: {
                                first: homeAddress.ADR_ZIP_CD,
                                last: homeAddress.ADR_ZIP_EXTN
                            },
                        },
                        mailingAddress: {
                            street1: mailingAddress.ADR_LINE1,
                            street2: mailingAddress.ADR_LINE2,
                            city: mailingAddress.ADR_CITY,
                            state: mailingAddress.ADR_STATE_CD,
                            zip: {
                                first: mailingAddress.ADR_ZIP_CD,
                                last: mailingAddress.ADR_ZIP_EXTN
                            },
                        },
                        mailing: (data.result.MAILING_ADR === true) ? 'same' : 'other',
                    }
                },
                login: {
                    username: data.result.USERNAME,
                    password: '',
                    retype: '',
                    question1: data.result.QUESTION_1,
                    answer1: data.result.ANSWER_1,
                    question2: data.result.QUESTION_2,
                    answer2: data.result.ANSWER_2,
                },
                PersonProfilePic: self.ParentProfileList,
                captcha: true
            };


        });
    }

    self.checkUsername = function () {
        self.usernameTaken = false;
        $resource("http://localhost:8085/api/Users/id/:id/type/:type", { id: self.parentProfile.login.username, type:0 }).get(function (data) {
            if (data.result === true)
                self.usernameTaken = true;
        });
    }

    if (self.isRegistering) {
        self.registrationObj = $cookieStore.get('registrationObj');
		$rootScope.tabset[1].disabled = true;
        $rootScope.tabset[2].disabled = true;
        $rootScope.tabset[3].disabled = true;
        self.enableEdit = false;
    }
    else {
        $scope.userObj = $cookieStore.get('Userdata');
        self.token = $scope.userObj.result.TokenId;
        self.PERS_ID = $scope.userObj.result.PERS_ID;
         self.getParent(self.PERS_ID, self.token);
    }



    self.parentProfile = {
        parent: {
            firstname: (self.isRegistering) ? self.registrationObj.firstname : '',
            middlename: '',
            lastname: (self.isRegistering) ? self.registrationObj.lastname : '',
            gender: 'M',
            social: {
                first: '',
                middle: '',
                last: ''
            },
        },
        contact: {
            home: {
                first: '',
                middle: '',
                last: ''
            },
            work: {
                first: '',
                middle: '',
                last: ''
            },
            mobile: {
                first: '',
                middle: '',
                last: ''
            },
            email: (self.isRegistering) ? self.registrationObj.email : '',
            bestTimeToCall: '',
            address: {
                homeAddress: {
                    street1: '',
                    street2: '',
                    city: '',
                    state: 'CA',
                    zip: {
                        first: '',
                        last: ''
                    },
                },
                mailingAddress: {
                    street1: '',
                    street2: '',
                    city: '',
                    state: '',
                    zip: {
                        first: '',
                        last: ''
                    },
                },
                mailing: 'same'
            }
        },
        login: {
            username: '',
            oldpassword: '',
            password: '',
            retype: '',
            question1: '',
            answer1: '',
            question2: '',
            answer2: '',
        },
        captcha: true
    };



    self.questions = [{ id: 1, question: "What was your first pet’s name?" },
                  { id: 2, question: "Which phone number do you remember most from your childhood?" },
                  { id: 3, question: "What is the first and last name of your first boyfriend or girlfriend?" },
                  { id: 4, question: "What was your favorite place to visit as a child?" },
                  { id: 5, question: "Who is your favorite actor, musician, or artist?" },
    ];

    self.questions2 = [
              { id: 6, question: "In what city were you born?" },
              { id: 7, question: "What high school did you attend?" },
              { id: 8, question: "What is the name of your first school?" },
              { id: 9, question: "What is your favorite movie?" },
              { id: 10, question: "What is your mother's maiden name?" },
    ];


    self.showmailing = true;

    self.showMailing = function (value) {
        self.parentProfile.contact.address.mailing = value;
        self.showmailing = (value === 'same') ? true : false;
    }

    self.setGender = function (value) {
        self.parentProfile.parent.gender = value;
    }

    self.formstatus = {
        show: false,
        type: '',
        message:''
    };

    self.saveParentProfile = function () {
        self.formstatus.show = false;
        self.validationError = false;
        var Parent_Profile = [];
        angular.forEach(self.ParentProfileList, function (value, key) {
            Parent_Profile.push(
            {
                DIS_PIC_ID: value.name,
                Profile_File_Name: "Profile" 
            });
        });
        if ($scope.parentform.$valid) {
            var profilePro = {
                USERNAME: self.parentProfile.login.username,
                PASSWORD: self.parentProfile.login.password,
                QUESTION_1: self.parentProfile.login.question1,
                ANSWER_1: self.parentProfile.login.answer1,
                QUESTION_2: self.parentProfile.login.question2,
                ANSWER_2: self.parentProfile.login.answer2,
                MAILING_ADR: (self.parentProfile.contact.address.mailing === 'same') ? true : false,
                personname: {
                    PERS_FNAME: self.parentProfile.parent.firstname,
                    PERS_LNAME: self.parentProfile.parent.lastname,
                    PERS_MNAME: self.parentProfile.parent.middlename,
                    PERS_NAME_SFIX: '',
                    PERS_NAME_PFIX: '',
                    PERS_NM_ID: ''
                },
                PhoneTypes: [{
                    PHON_TYPE_CD: 'home',
                    PHON_TYP_DESC: self.parentProfile.contact.home.first + self.parentProfile.contact.home.middle + self.parentProfile.contact.home.last,
                }, {
                    PHON_TYPE_CD: 'work',
                    PHON_TYP_DESC: self.parentProfile.contact.work.first + self.parentProfile.contact.work.middle + self.parentProfile.contact.work.last,
                },
                {
                    PHON_TYPE_CD: 'mobile',
                    PHON_TYP_DESC: self.parentProfile.contact.mobile.first + self.parentProfile.contact.mobile.middle + self.parentProfile.contact.mobile.last,
                }
                ],
                Person: {
                    email: self.parentProfile.contact.email,
                    TIMETOCALL: self.parentProfile.contact.bestTimeToCall,
                    Gender: self.parentProfile.parent.gender,
                    Social_Security_No: self.parentProfile.parent.social.first + self.parentProfile.parent.social.middle + self.parentProfile.parent.social.last
                },
                Address: [{
                    ADR_LINE1: self.parentProfile.contact.address.homeAddress.street1,
                    ADR_LINE2: self.parentProfile.contact.address.homeAddress.street2,
                    ADR_CITY: self.parentProfile.contact.address.homeAddress.city,
                    ADR_STATE_CD: self.parentProfile.contact.address.homeAddress.state,
                    ADR_CNTRY: '',
                    ADR_ZIP_CD: self.parentProfile.contact.address.homeAddress.zip.first,
                    ADR_ZIP_EXTN: self.parentProfile.contact.address.homeAddress.zip.last,
                    MAILING_ADR: (self.parentProfile.contact.address.mailing === 'same') ? true : false,
                    ADR_DESC: 'home'
                },
                {
                    ADR_LINE1: self.parentProfile.contact.address.mailingAddress.street1,
                    ADR_LINE2: self.parentProfile.contact.address.mailingAddress.street2,
                    ADR_CITY: self.parentProfile.contact.address.mailingAddress.city,
                    ADR_STATE_CD: self.parentProfile.contact.address.mailingAddress.state,
                    ADR_CNTRY: '',
                    ADR_ZIP_CD: self.parentProfile.contact.address.mailingAddress.zip.first,
                    ADR_ZIP_EXTN: self.parentProfile.contact.address.mailingAddress.zip.last,
                    MAILING_ADR: (self.parentProfile.contact.address.mailing === 'same') ? true : false,
                    ADR_DESC: 'mailing'
                }
                ],
                PersonProfilePic: Parent_Profile,
            };

            $resource("http://localhost:8085/api/Register").save(profilePro, function (data) {
                if (data.result !== '00000000-0000-0000-0000-000000000000') {
                    $cookieStore.put('session', { loggedIn: true, registering: false });
                    self.isRegistering = helper.isRegistering($cookieStore.get('session'));
                    $cookieStore.put('Userdata', data);
                    self.token = data.result.TokenId;
                    self.PERS_ID = data.result.PERS_ID;
                    self.getParent(self.PERS_ID, self.token);
                    self.formstatus = {
                        show: true,
                        type: 'success',
                        message: 'Profile Created Successfully'
                    };
					$rootScope.tabset[1].disabled = false;
                    $rootScope.tabset[2].disabled = false;
                    $rootScope.tabset[3].disabled = false;
                    $scope.passwordBar = 0;
                } else {
                    self.formstatus = {
                        show: true,
                        type: 'danger',
                        message: 'Something went wrong, try again later'
                    };
                }
                $window.scrollTo(0, 0);

            });
            self.submitted = false;
        } else {
            self.submitted = true;
            self.validationError = true;
            $window.scrollTo(0, 0);
        }
    }

    self.updateParent = function () {
        var Parent_Profile = [];
        angular.forEach(self.ParentProfileList, function (value, key) {
          
            Parent_Profile.push(
            {
                DIS_PIC_ID: value.name,
                Profile_File_Name: 'Profile'
            });
           
        });
        self.formstatus.show = false;
        self.validationError = false;
        if ($scope.parentform.$valid) {
            var profilePro = {
                PERS_ID: self.PERS_ID,
                USERNAME: self.parentProfile.login.username,
                PASSWORD: self.parentProfile.login.password,
                OLDPASSWORD: self.parentProfile.login.oldpassword,
                QUESTION_1: self.parentProfile.login.question1,
                ANSWER_1: self.parentProfile.login.answer1,
                QUESTION_2: self.parentProfile.login.question2,
                ANSWER_2: self.parentProfile.login.answer2,
                MAILING_ADR: (self.parentProfile.contact.address.mailing === 'same') ? true : false,
                personname: {
                    PERS_FNAME: self.parentProfile.parent.firstname,
                    PERS_LNAME: self.parentProfile.parent.lastname,
                    PERS_MNAME: self.parentProfile.parent.middlename,
                    PERS_NAME_SFIX: '',
                    PERS_NAME_PFIX: '',
                    PERS_NM_ID: ''
                },
                PhoneTypes: [{
                    PHON_TYPE_CD: 'home',
                    PHON_TYP_DESC: self.parentProfile.contact.home.first + self.parentProfile.contact.home.middle + self.parentProfile.contact.home.last,
                }, {
                    PHON_TYPE_CD: 'work',
                    PHON_TYP_DESC: self.parentProfile.contact.work.first + self.parentProfile.contact.work.middle + self.parentProfile.contact.work.last,
                },
                {
                    PHON_TYPE_CD: 'mobile',
                    PHON_TYP_DESC: self.parentProfile.contact.mobile.first + self.parentProfile.contact.mobile.middle + self.parentProfile.contact.mobile.last,
                }
                ],
                Person: {
                    email: self.parentProfile.contact.email,
                    TIMETOCALL: self.parentProfile.contact.bestTimeToCall,
                    Gender: self.parentProfile.parent.gender,
                    Social_Security_No: self.parentProfile.parent.social.first + self.parentProfile.parent.social.middle + self.parentProfile.parent.social.last
                },
                Address: [{
                    ADR_LINE1: self.parentProfile.contact.address.homeAddress.street1,
                    ADR_LINE2: self.parentProfile.contact.address.homeAddress.street2,
                    ADR_CITY: self.parentProfile.contact.address.homeAddress.city,
                    ADR_STATE_CD: self.parentProfile.contact.address.homeAddress.state,
                    ADR_CNTRY: '',
                    ADR_ZIP_CD: self.parentProfile.contact.address.homeAddress.zip.first,
                    ADR_ZIP_EXTN: self.parentProfile.contact.address.homeAddress.zip.last,
                    MAILING_ADR: (self.parentProfile.contact.address.mailing === 'same') ? true : false,
                    ADR_DESC: 'home'
                },
                {
                    ADR_LINE1: self.parentProfile.contact.address.mailingAddress.street1,
                    ADR_LINE2: self.parentProfile.contact.address.mailingAddress.street2,
                    ADR_CITY: self.parentProfile.contact.address.mailingAddress.city,
                    ADR_STATE_CD: self.parentProfile.contact.address.mailingAddress.state,
                    ADR_CNTRY: '',
                    ADR_ZIP_CD: self.parentProfile.contact.address.mailingAddress.zip.first,
                    ADR_ZIP_EXTN: self.parentProfile.contact.address.mailingAddress.zip.last,
                    MAILING_ADR: (self.parentProfile.contact.address.mailing === 'same') ? true : false,
                    ADR_DESC: 'mailing'
                }
                ],
                PersonProfilePic: Parent_Profile,
            };

            $resource("http://localhost:8085/api/Register/:token", { token: self.token }, { 'update': { method: 'PUT' } }).update(profilePro, function (data) {
                if(data.result === true)
                 {
                    self.formstatus = {
                        show: true,
                        type: 'success',
                        message: 'Profile Updated Successfully'
                    };
					self.changePassword = false;
                    self.hideChangePassword = false;
                     self.parentProfile.login.password = '';
                     self.parentProfile.login.oldpassword = ''; 
                     self.parentProfile.login.retype = '';
                     $scope.passwordBar = 0;
                     self.enableEdit = true;
					 // self.enableEdit = false;
                }
                else
                {
                    self.formstatus = {
                        show: true,
                        type: 'danger',
                        message: 'Something went wrong, try again later'
                    };
                }
                $window.scrollTo(0, 0);
            });
        } else {
            self.submitted = true;
            self.validationError = true;
            $window.scrollTo(0, 0);
        }
    }



    $scope.passwordBar = 0;
    $scope.passwordBarType = 'success';

    self.passwordValidator = function (password) {
        $scope.passwordBar = 0;
        var hasNumber = /[^a-zA-Z]/;
        var lowercase = /[a-z]/;
        var uppercase = /[A-Z]/;
        if (password === undefined)
            return;
        if (hasNumber.test(password))
            $scope.passwordBar = 25;
        if (lowercase.test(password))
            $scope.passwordBar = $scope.passwordBar + 50;
        if (uppercase.test(password))
            $scope.passwordBar = $scope.passwordBar + 25;
    }

    $scope.getCaptchaResult = function (result) {
        if (result === 'success')
            self.parentProfile.captcha = false;
    }

    self.passwordMatch = function () {
        if (self.parentProfile.login.retype === self.parentProfile.login.password) {
            $scope.passwordBarType = 'success';
        }
        else {
            $scope.passwordBarType = 'danger';
        }

    }

    self.DeleteFile = function (FileId, Type, index) {
        $scope.userObj = $cookieStore.get('Userdata');
        $resource("http://localhost:8085/api/Children", { id: $scope.userObj.result.PERS_ID, token: $scope.userObj.result.TokenId, Filename: FileId, Type: Type }).delete(function (data) {
            if (data.result == true) {              
                    self.ParentProfileList.splice(index, 1);              
                //self.ParentProfileList = [];
            }

        });
    }
    return self;

}]);