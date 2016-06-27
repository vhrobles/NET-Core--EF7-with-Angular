(function () {
    'use strict';
    var portfolioFactory = function ($http) {

        var baseUrl = '/api/portfolioService';
        var factory = {};

        factory.getCustomerPortfolio = function () {

            return $http({
                url: baseUrl + '/',
                method: 'GET'                
            });
        };

        factory.getCustomerPortfolio = function (id) {

            return $http({
                url: baseUrl + '/' + id,
                method: 'GET'
            });
        };

        factory.getCurrentCustomerPortfolio = function () {

            return $http({
                url: baseUrl + '/',
                method: 'GET'
            });
        };

        factory.getAllPortfolios = function() {
            return $http({
                url: baseUrl + '/GetAll',
                method: 'GET'
            });
        };

        factory.getAllSecurityTypes = function () {
            return $http({
                url: baseUrl + '/GetAllSecurityTypes',
                method: 'GET'
            });
        };
        factory.UpdateSecurityType = function(id, calculation) {
            return $http({
                url: baseUrl + '/UpdateSecurityType',
                method: 'PUT',
                params: { id: id, calculation: calculation } 
            });
        };
        factory.addSecurity = function(customerSecurity) {
            return $http({
                url: baseUrl + '/',
                dataType: 'json',
                method: 'POST',                                
                data: JSON.stringify(customerSecurity),
                headers: { 'Content-Type': 'application/json' }
            });
        };

        return factory;
    };

    var portfolioDetailsController = function($scope,
        $http,
        $resource,
        $location,
        $routeParams,
        portfolioFactory,
        NgTableParams,
        ModalService) {
        
        $scope.addSecurityForm = {};
        var match = /\/Details\/(\d+)/.exec($location.$$absUrl);

        $scope.customerId = match[1];
        $scope.loading = true;

        
            portfolioFactory.getCustomerPortfolio($scope.customerId)
                .then(function(response) {
                        $scope.portfolio = response.data;

                        $scope.loading = false;

                        $scope.customerTable = new NgTableParams({
                            group: 'type'
                        },
                        {
                            dataset: $scope.portfolio.customerSecurities
                        });

                    },
                    function(error) {

                    });
        
            
       
        $scope.addSecurity = function (customerId, portfolioId, symbol, type, price) {
            $scope.modalMessage = "Are you sure you want to add this security?";
            $scope.customerSecurity = {
                CustomerId: customerId,
                PortfolioId: portfolioId,
                Price: price,
                Symbol: symbol,
                Type: type                                
            };
            ModalService.showModal({
                templateUrl: "modal.html",
                controller: "modalController",
                scope: $scope
            }).then(function (modal) {
                modal.element.body = 'aaaaaaa';
                //it's a bootstrap element, use 'modal' to show it
                modal.element.modal();
                modal.close.then(function (result) {
                    
                });
            });
        }

        $scope.saveSecurity = function() {
            portfolioFactory.addSecurity($scope.customerSecurity)
                .then(function(response) {
                    portfolioFactory.getCustomerPortfolio($scope.customerId)
                   .then(function (response) {
                       $scope.portfolio = response.data;

                       $scope.loading = false;

                       $scope.customerTable = new NgTableParams({
                           group: 'type'
                       },
                       {
                           dataset: $scope.portfolio.customerSecurities
                       });
                        $('#addSecurityForm')[0].reset();
                    },
                    function (error) {

                    });
                },
                function(error) {
                    
                }
            );
        }
    }

    var portfolioController = function ($scope, $http, $resource, $location, portfolioFactory, NgTableParams) {       
        $scope.loading = true;                
        var self = this;        
        portfolioFactory.getCurrentCustomerPortfolio()
            .then(function (response) {
                $scope.portfolio = response.data;                
                
                $scope.loading = false;

                $scope.customerTable = new NgTableParams({
                    group: 'type'
                }, {
                    dataset: $scope.portfolio.customerSecurities
                });
                
                

                
            },

                function (error) {

                });
    }

    var managePortfoliosController = function ($scope, $http, $resource, $location, portfolioFactory, NgTableParams) {
       
        $scope.loading = true;       
        
        portfolioFactory.getAllPortfolios()
            .then(function (response) {
                $scope.portfolios = response.data;                                                
                $scope.customerTable = new NgTableParams({}, {
                    dataset: $scope.portfolios
                });
                
                $scope.loading = false;
            },
            function (error) {

            });

    }

    var securityTypesController = function($scope, $http, $resource, portfolioFactory, NgTableParams) {
        $scope.loading = true;
        var self = this;
        portfolioFactory.getAllSecurityTypes()
            .then(function (response) {
                $scope.securityTypes = response.data;
                $scope.originalData = angular.copy( response.data);
                $scope.customerTable = new NgTableParams({}, {
                    filterDelay: 0,
                    dataset: angular.copy($scope.securityTypes)
                });


                $scope.loading = false;
            },
            function (error) {

            });
           
       
        $scope.cancel = cancel;        
        $scope.save = save;


        function cancel(row, rowForm) {
            var originalRow = resetRow(row, rowForm);
            angular.extend(row, originalRow);
        }
        

        function resetRow(row, rowForm) {
            row.isEditing = false;
            rowForm.$setPristine();
            
            for (let i in $scope.originalData) {
                if ($scope.originalData.hasOwnProperty(i)) {
                    if ($scope.originalData[i].id === row.id) {
                        return $scope.originalData[i];
                    }
                }
            }
        }
        
        function save(row, rowForm) {            
            var originalRow = resetRow(row, rowForm);
            angular.extend(originalRow, row);
            $scope.loading = true;
            portfolioFactory.UpdateSecurityType(row.type, row.calculation)
            .then(function (response) {                

                $scope.loading = false;
            },
            function (error) {

            });
        }
    }

    angular
        .module('guidantApp.portfolio', ['ngRoute', 'ngResource', 'ngMessages'])
        .controller('portfolioController', portfolioController)        
        .controller('managePortfoliosController', managePortfoliosController)
        .controller('portfolioDetailsController', portfolioDetailsController)
        .controller('securityTypesController', securityTypesController)
        .factory('portfolioFactory', portfolioFactory)
        .filter('securityType', function() {
            return function(type) {
                var pType;
                switch (type) {
                    case 1:
                        pType = 'Funds';
                        break;
                    case 2:
                        pType = 'Stocks';
                        break;
                    case 3:
                        pType = 'Bonds';
                        break;
                    default:
                        pType = '';
                        break;
                }
                return pType;
            };
        });
    
})();
