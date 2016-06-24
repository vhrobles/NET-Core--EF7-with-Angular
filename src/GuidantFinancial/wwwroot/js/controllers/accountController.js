(function () {
    'use strict';
    var accountFactory = function ($http) {

        var baseUrl = '/api/accountService';
        var factory = {};

        factory.registerCustomer = function (customer) {
            
            return $http({
                url: baseUrl + '/',
                dataType: 'json',
                method: 'POST',
                data: customer,
                headers: { 'Content-Type': 'application/json' }
            });
        };

        return factory;
    };

    var accountController = function ($scope, $http, $resource, $location, accountFactory) {

        $scope.submitForm = function () {
            
            if ($scope.registerForm.$invalid) {
                return;
            }
           
            var customer = {                
                Email: $scope.email,                             
                Password: $scope.password,
                Portfolio: {
                    Id: 0,
                    Name: '',
                    Securities: null
                }
            }
            
            $scope.success = false;
            accountFactory.registerCustomer(customer).then(function (response) {
                    $scope.success = true;
                    $scope.response = response;                    

                },
                function (error) {

                });
        }

        

    }

    var compareTo = function () {
        return {
            require: "ngModel",
            scope: {
                otherModelValue: "=compareTo"
            },
            link: function (scope, element, attributes, ngModel) {

                ngModel.$validators.compareTo = function (modelValue) {
                    return modelValue == scope.otherModelValue;
                };

                scope.$watch("otherModelValue", function () {
                    ngModel.$validate();
                });
            }
        };
    };

    angular
        .module('guidantApp.account', ['ngRoute', 'ngResource', 'ngMessages', 'ngAnimate'])
        .controller('accountController', accountController)
        .factory('accountFactory', accountFactory)
        .directive('compareTo', compareTo);

    

   

    
})();
