(function () {
    'use strict';
    var portfolioService = function ($http) {

        var baseUrl = '/api/portfolioService';
        var portfolioFactory = {};

        portfolioFactory.getCustomerPortfolio = function () {

            return $http({
                url: baseUrl + '/',
                method: 'GET'                
            });
        };

        return portfolioFactory;
    };

    var portfolioController = function ($scope, $http, $resource, $location, portfolioService) {
        $scope.sortByColumn = 'type';
        portfolioService.getCustomerPortfolio()
            .then(function (response) {
                $scope.portfolio = response.data;
            },
                function (error) {

                });
    }

    angular
        .module('guidantApp.portfolio', ['ngRoute', 'ngResource'])
        .controller('portfolioController', portfolioController)
        .factory('portfolioService', portfolioService);

    

   

    
})();
