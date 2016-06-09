app.controller('childProfileController', ['$scope', 'service', '$rootScope', '$cookieStore', 'helper', '$resource', '$filter', 'uibDateParser', '$timeout', '$window', function ($scope, service, $rootScope, $cookieStore, helper, $resource, $filter, uibDateParser, $timeout, $window) {

    var self = Object.create(null);
    self.helper = helper;


    self.child = {
        childProfileLists: [
        ],
        ethnicityList: [
            { id: 1, name: 'Hispanic or Latino' },
            { id: 2, name: 'American Indian or Alaska Native' },
            { id: 3, name: 'Asian' },
            { id: 4, name: 'Black or African American' },
            { id: 5, name: 'Native Hawaiian or Other Pacific Islander' },
            { id: 6, name: 'White' },
        ],
        relationList: [
            { id: 1, name: 'Son' },
            { id: 2, name: 'Daughter' },
            { id: 3, name: 'Step Son' },
            { id: 4, name: 'Step Daughter' },
        ],
        DocumentList: [
          { id: 1, name: 'Birth Certificate' },
          { id: 2, name: 'Adoption Certificate' }
        ],
        disabilityList: [
            { id: 1, name: 'Mental Retardation' },
            { id: 2, name: 'Visually or Hearing Impaired' },
            { id: 3, name: 'Physically Disabled' },
            { id: 4, name: 'Emotionally Disturbed' },
            { id: 5, name: 'Other Medically Diagnosed Condition Requiring Special Care' },
        ],
        disabilityType: [
           { id: 1, name: 'Clinical Diagnosis' },
           { id: 2, name: 'Lab Test' },
        ],
        ChildProfileList: [],
        childProfile: '',
        childInformation: {
            firstname: '',
            middlename: '',
            lastname: '',
            gender: 'M',
            ethnicity: '',
            dob: '',
            social: {
                first: '',
                second: '',
                third: '',
            },
            photos: {

            },
        },
        relationshipInformation: {
            relation: '',
            proofDocuments: [],
            genogram: {},
            DocumentType: ''
        },
        disabilityInformation: {
            type: '',
            documents: [],
            note: '',
            UploadDocType: '',
            DIS_Document: ''
        },
        PersonProfilePic: []

    }
    self.isRegistering = false;
    self.masterchild = angular.copy(self.child);
    self.IsDisableUpload = false;
    self.IsProfileUpload = false;
    self.invalidDisabilityfile = false;
    self.invalidRelationShipfile = false;
    self.invalidProfilefile = false;


    self.childProfileDisable = false;
    $scope.$on('refreshDisability', function (event, args) {

        $scope.$apply(function () {
            self.child.disabilityInformation.documents.push({ name: args.name, filename: args.filename, type: (self.child.disabilityInformation.UploadDocType === 1) ? 'Clinical Diagnosis' : 'Lab Test', typeId: self.child.disabilityInformation.UploadDocType });
            self.IsDisableUpload = true;
            $timeout(function () {
                self.IsDisableUpload = false;                
            }, 3000);
        });
    });
    $scope.$on('refreshInvalidDisabilityPdf', function (event, args) {
        $scope.$apply(function () {
            self.invalidDisabilityfile = true;
            $timeout(function () {
                self.invalidDisabilityfile = false;
            }, 4000);
        });
      
    });
    $scope.$on('refreshInvalidRelationShipPdf', function (event, args) {
        $scope.$apply(function () {
            self.invalidRelationShipfile = true;
            $timeout(function () {
                self.invalidRelationShipfile = false;
            }, 4000);
        });

    });
    
    $scope.$on('refreshRelationship', function (event, args) {
        $scope.$apply(function () {
            self.child.relationshipInformation.proofDocuments.push({ name: args.name, filename: args.filename, type: (self.child.relationshipInformation.DocumentType === 1) ? 'Birth Certificate' : 'Adoption Certificate', typeId: self.child.relationshipInformation.DocumentType });
            self.IsRelationUpload = true;
            $timeout(function () {
                self.IsRelationUpload = false;
               
            }, 3000);
        });
    });

    $scope.$on('refreshProfilePhoto', function (event, args) {
        $scope.$apply(function () {
            self.child.ChildProfileList.push({ name: args.name, image: args.Image });
            self.IsProfileUpload = true;
            $timeout(function () {
                self.IsProfileUpload = false;              
            }, 3000);
        });
    });

    self.addChild = function () {
        self.isRegistering = false;
        var child = self.child.childProfileLists;
        self.child = angular.copy(self.masterchild);
        self.getChildListWithout();
		self.childProfileDisable = false;
    }
    self.closeAlert = function () {
        self.IsDisableUpload = false;
        self.IsProfileUpload = false;
        self.IsRelationUpload = false;
    };
    self.GetRelation = function (val) {
        return (val == 1) ? 'Son' : (val == 2) ? 'Daughter' : (val == 3) ? 'Step Son' : 'Step Daughter';
    }

    $scope.userObject = $cookieStore.get('Userdata');


    self.getChildInfo = function () {

        $scope.userObj = $cookieStore.get('Userdata');
        $resource("http://localhost:8085/api/Children/:token/:id", { id: self.child.childProfile, token: $scope.userObj.result.TokenId, PERS_Id: $scope.userObj.result.PERS_ID }).get(function (data) {
            self.childProfileDisable = true;
            var date = $filter('date')(data.result.Person.DOB, 'dd/MM/yyyy');


            self.child.childInformation.firstname = data.result.personname.PERS_FNAME;
            self.child.childInformation.middlename = data.result.personname.PERS_MNAME;
            self.child.childInformation.lastname = data.result.personname.PERS_LNAME;
            self.child.childInformation.gender = data.result.Person.Gender;
            self.child.childInformation.ethnicity = data.result.Person.Race_Ethencity;
            self.child.childInformation.dob = new Date(data.result.Person.DOB);
            self.child.childInformation.social.first = data.result.Person.Social_Security_No.substring(0, 3);
            self.child.childInformation.social.second = data.result.Person.Social_Security_No.substring(3, 5);
            self.child.childInformation.social.third = data.result.Person.Social_Security_No.substring(5, 9);
            self.child.relationshipInformation.relation = data.result.RelationshipMap.Relationship_Code;
            self.child.disabilityInformation.type = data.result.child_disability[0].DIS_TypeId;
            self.child.disabilityInformation.note = data.result.child_disability[0].NOTE;

            self.child.disabilityInformation.documents = [];
            self.child.relationshipInformation.proofDocuments = [];
            self.child.disabilityInformation.documents = [];
            self.child.ChildProfileList = [];
            angular.forEach(data.result.child_disability, function (value, key) {
                self.child.disabilityInformation.documents.push({ name: value.DIS_Document, filename: value.DIS_File_Name, type: (value.DIS_UploadDoc_TypeId === 1) ? 'Clinical Diagnosis' : 'Lab Test', typeId: value.DIS_UploadDoc_TypeId });
            });

            angular.forEach(data.result.child_relationship, function (value, key) {
                self.child.relationshipInformation.proofDocuments.push({
                    name: value.Proof_Doc, filename: value.Proof_File_Name, type: (self.child.relationList === 1) ? 'Birth Certificate' : 'Adoption Certificate', typeId: value.Proof_Type
                });
            });

            angular.forEach(data.result.PersonProfilePic, function (value, key) {
                self.child.ChildProfileList.push({ name: value.DIS_PIC_ID, image: value.image });
            });


        });
        self.isRegistering = true;
    }

    self.getChildList = function () {
        
        $scope.userObj = $cookieStore.get('Userdata');
        $resource("http://localhost:8085/api/Child/:token/:id", { id: $scope.userObj.result.PERS_ID, token: $scope.userObj.result.TokenId }).get(function (data) {
            self.child.childProfileLists = data.result;
         
            if (self.child.childProfileLists.length > 0) {
                self.child.childProfile = data.result[0].personname.PERS_ID;
                //var result = $filter('filter')(foo.results)[0];
                self.getChildInfo();
            }
            
        });

    };
	
	    self.getChildListWithout = function () {
        
        $scope.userObj = $cookieStore.get('Userdata');
        $resource("http://localhost:8085/api/Child/:token/:id", { id: $scope.userObj.result.PERS_ID, token: $scope.userObj.result.TokenId }).get(function (data) {
            self.child.childProfileLists = data.result;
        
            
        });

    };
	
    self.getChildList();
    self.opened = false;

    self.openDatepicker = function () {
        self.opened = true;
    };

    self.format = 'MM/dd/yyyy';

    self.dateOptions = {
        dateDisabled: false,
        formatYear: 'yy',
        maxDate: new Date(),
        minDate: new Date(1100, 1, 1),
        startingDay: 1,
		showWeeks:false
    };

    self.formstatus = {
        show: false,
        type: '',
        message: ''
    };


    self.saveChildProfile = function () {
        self.formstatus.show = false;
		self.validationError = false;
        if ($scope.childform.$valid) {

            var child_disability = [];
            var child_RelationShip = [];
            var child_ProfilePic = [];
            angular.forEach(self.child.disabilityInformation.documents, function (value, key) {
                child_disability.push(
                {
                    PERS_ID: $scope.userObj.result.PERS_ID,
                    DIS_TypeId: self.child.disabilityInformation.type,
                    DIS_UploadDoc_TypeId: value.typeId,
                    DIS_Document: value.name,
                    DIS_File_Name: value.filename,
                    NOTE: self.child.disabilityInformation.note
                });

            });

            angular.forEach(self.child.relationshipInformation.proofDocuments, function (value, key) {
                child_RelationShip.push(
                {
                    Proof_Type: value.typeId,
                    Proof_Doc: value.name,
                    Proof_File_Name: value.filename
                });

            });

            angular.forEach(self.child.ChildProfileList, function (value, key) {
                child_ProfilePic.push(
                {
                    DIS_PIC_ID: value.name,
                    Profile_File_Name:"Profile"
                });

            });

          
           
            $scope.userObj = $cookieStore.get('Userdata');
            
         
            var profilePro = {

                PERS_ID: $scope.userObj.result.PERS_ID,
                personname: {
                    PERS_FNAME: self.child.childInformation.firstname,
                    PERS_LNAME: self.child.childInformation.lastname,
                    PERS_MNAME: self.child.childInformation.middlename,
                    PERS_NAME_SFIX: '',
                    PERS_NAME_PFIX: '',
                    PERS_NM_ID: ''
                },

                Person: {
                    Race_Ethencity: self.child.childInformation.ethnicity,
                    DOB: self.child.childInformation.dob,
                    Gender: self.child.childInformation.gender,
                    Social_Security_No: self.child.childInformation.social.first + self.child.childInformation.social.second + self.child.childInformation.social.third,
                    IsParent: false
                },
                child_relationship: child_RelationShip,
                child_disability: child_disability,
                PersonProfilePic: child_ProfilePic,
                RelationshipMap:
                    {
                        Relationship_Code: self.child.relationshipInformation.relation
                    }               

            };

            var tokenId = ($scope.userObj.TokenId === undefined) ? $scope.userObj.result.TokenId : $scope.userObj.TokenId;
            $resource("http://localhost:8085/api/Children", { token: tokenId }).save(profilePro, function (data) {
                if (data.result !== '00000000-0000-0000-0000-000000000000') {
                    $cookieStore.put('session', { loggedIn: true, registering: false });
                    self.isRegistering = helper.isRegistering($cookieStore.get('session'));
                    self.getChildList();
                    //self.addChild();
                    self.isRegistering = true;
                    self.formstatus = {
                        show: true,
                        type: 'success',
                        message: 'Child Profile Created Successfully'
                    };
                    self.child.childProfile = data.result;
                } else {
                    self.formstatus = {
                        show: true,
                        type: 'danger',
                        message: 'Something went wrong, try again later'
                    };
                }
                self.submitted = false;
                self.validationError = false;
                $window.scrollTo(0, 0);

            });
        } else {
            self.submitted = true;
            self.validationError = true;
        }

    };

    self.DeleteFile = function (FileId, Type, index) {
        $scope.userObj = $cookieStore.get('Userdata');
        $resource("http://localhost:8085/api/Children", { id: $scope.userObj.result.PERS_ID, token: $scope.userObj.result.TokenId, Filename: FileId, Type: Type }).delete(function (data) {
            if (data.result == true) {
                if (Type === 'Disability') {
                    self.child.disabilityInformation.documents.splice(index, 1);
                }
                else if (Type === 'Relationship') {
                    self.child.relationshipInformation.proofDocuments.splice(index, 1);
                }
                else if (Type === "Profile") {
                    self.child.ChildProfileList.splice(index, 1);
                }

            }

        });
    }
    self.updateChild = function () {
        self.formstatus.show = false;
		self.validationError = false;
        var child_disability = [];
        var child_RelationShip = [];
        var child_ProfilePic = [];
        angular.forEach(self.child.disabilityInformation.documents, function (value, key) {
            child_disability.push(
            {
                PERS_ID: $scope.userObj.result.PERS_ID,
                DIS_TypeId: self.child.disabilityInformation.type,
                DIS_UploadDoc_TypeId: value.typeId,
                DIS_Document: value.name,
                DIS_File_Name: value.filename,
                NOTE: self.child.disabilityInformation.note
            });

        });

        angular.forEach(self.child.relationshipInformation.proofDocuments, function (value, key) {
            child_RelationShip.push(
            {
                Proof_Type: value.typeId,
                Proof_Doc: value.name,
                Proof_File_Name: value.filename
            });

        });

        angular.forEach(self.child.ChildProfileList, function (value, key) {
           
            child_ProfilePic.push(
            {
                DIS_PIC_ID: value.name,
                Profile_File_Name: 'Profile'
            });
            return false;
        });

        $scope.userObj = $cookieStore.get('Userdata');
        if ($scope.childform.$valid) {
            var profilePro = {
                ChildID: self.child.childProfile,
                PERS_ID: $scope.userObj.result.PERS_ID,
                personname: {
                    PERS_FNAME: self.child.childInformation.firstname,
                    PERS_LNAME: self.child.childInformation.lastname,
                    PERS_MNAME: self.child.childInformation.middlename,
                    PERS_NAME_SFIX: '',
                    PERS_NAME_PFIX: '',
                    PERS_NM_ID: ''
                },

                Person: {
                    Race_Ethencity: self.child.childInformation.ethnicity,
                    DOB: self.child.childInformation.dob,
                    Gender: self.child.childInformation.gender,
                    Social_Security_No: self.child.childInformation.social.first + self.child.childInformation.social.second + self.child.childInformation.social.third,
                    IsParent: false
                },
                child_relationship: child_RelationShip,
                child_disability: child_disability,
                PersonProfilePic: child_ProfilePic,
                RelationshipMap:
                    {
                        Relationship_Code: self.child.relationshipInformation.relation
                    }



            };
            var tokenId = ($scope.userObj.TokenId === undefined) ? $scope.userObj.result.TokenId : $scope.userObj.TokenId;
            $resource("http://localhost:8085/api/Children/:token", { token: tokenId }, { 'update': { method: 'PUT' } }).update(profilePro, function (data) {

                if (data.result)
                {
                    self.formstatus = {
                        show: true,
                        type: 'success',
                        message: 'Child Profile Updated Successfully'
                    };
                    self.getChildList();
					self.childProfileDisable = false;
                   // self.addChild();
                } else {
                    self.formstatus = {
                        show: true,
                        type: 'danger',
                        message: 'Something went wrong, try again later'
                    };
                }
                $window.scrollTo(0, 0);
            });
        }
        else {
            self.submitted = true;
            self.validationError = true;
        }

    }

    return self;

}]);



