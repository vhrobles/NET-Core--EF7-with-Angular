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

    var accountController = function ($scope, $http, $resource, $location, accountFactory, ModalService) {

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
                    $scope.response = response.data ? 'Added successfully!' : 'Couldn\'t add user, she may already exists or an error ocurred';

                    if ($scope.response)
                        $('#registerForm')[0].reset();

                    ModalService.showModal({
                        templateUrl: "modal.html",
                        controller: "modalController",
                        scope: $scope
                    }).then(function(modal) {
                        modal.element.body = 'aaaaaaa';
                        //it's a bootstrap element, use 'modal' to show it
                        modal.element.modal();
                        modal.close.then(function(result) {
                            console.log(result);
                        });
                    });


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
