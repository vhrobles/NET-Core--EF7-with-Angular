(function () {
    'use strict';


    var app = angular.module('guidantApp',
    [
        // Angular modules 
        'ngRoute',
        'ngMessages',
        'ngAnimate',
        // Custom modules 
        'guidantApp.portfolio',
        'guidantApp.account',        
        // 3rd Party Modules
        'angularModalService',
        'ngTable'
    ]);    
    app.factory('paginationFactory', function () {

        var pagination = {};

        pagination.getNew = function (perPage) {

            perPage = perPage === undefined ? 5 : perPage;

            var paginator = {
                numPages: 1,
                perPage: perPage,
                page: 0
            };

            paginator.prevPage = function () {
                if (paginator.page > 0) {
                    paginator.page -= 1;
                }
            };

            paginator.nextPage = function () {
                if (paginator.page < paginator.numPages - 1) {
                    paginator.page += 1;
                }
            };

            paginator.toPageId = function (id) {
                if (id >= 0 && id <= paginator.numPages - 1) {
                    paginator.page = id;
                }
            };

            return paginator;
        };

        return pagination;
    });
    
    app.filter('startFrom', function () {
        return function (input, start) {
            if (input === undefined) {
                return input;
            } else {
                return input.slice(+start);
            }
        };
    });

    app.filter('range', function () {
        return function (input, total) {
            total = parseInt(total);
            for (var i = 0; i < total; i++) {
                input.push(i);
            }
            return input;
        };
    });

    app.directive('blurToCurrency',
        function($filter) {
            return {
                scope: {
                    amount: '='
                },
                link: function(scope, el, attrs) {
                    el.val($filter('currency')(scope.amount));

                    el.bind('focus',
                        function() {
                            el.val(scope.amount);
                        });

                    el.bind('input',
                        function() {
                            scope.amount = el.val();
                            scope.$apply();
                        });

                    el.bind('blur',
                        function() {
                            el.val($filter('currency')(scope.amount));                            
                        });
                }
            }
        });

    app.directive('capitalize',
        function() {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, modelCtrl) {
                    var capitalize = function (inputValue) {
                        if (inputValue == undefined) inputValue = '';
                        var capitalized = inputValue.toUpperCase();
                        if (capitalized !== inputValue) {
                            modelCtrl.$setViewValue(capitalized);
                            modelCtrl.$render();
                        }
                        return capitalized;
                    }
                    modelCtrl.$parsers.push(capitalize);
                    capitalize(scope[attrs.ngModel]); 
                }
            };
        });
})();