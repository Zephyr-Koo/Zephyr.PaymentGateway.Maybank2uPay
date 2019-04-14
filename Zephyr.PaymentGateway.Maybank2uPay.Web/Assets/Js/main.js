var app = angular
            .module('app.paymentGateway.m2upay', [])
            .controller('M2uPaymentGatewayController', M2uPaymentGatewayController);

M2uPaymentGatewayController.$inject = ['$http', '$sce'];

function M2uPaymentGatewayController($http, $sce) {
    var vm = this;

    init();

    vm.onParamChange = function () {
        vm.hasJson = true;

        $http
            .post('/encrypted-json', {
                payload     : vm.param,
                environment : vm.environment
            })
            .then(function successCallback(response) {
                var encryptedJson = response.data;

                vm.result.actionUrl       = $sce.trustAsResourceUrl(encryptedJson.actionUrl);
                vm.result.encryptedString = encryptedJson.encryptedString;
                vm.result.encryptedJson   = JSON.stringify(encryptedJson, null, '\t');
            }, function errorCallback() {
                console.error('Error computing encrypted json...');
            });
    };

    function init() {
        vm.hasJson = false; // acts like isDirty

        vm.environment = environment_data_binded;

        vm.param = {
            amount    : '',
            accountNo : '',
            payeeCode : '',
            refNo     : ''
        };

        vm.result = {
            actionUrl       : '',
            encryptedString : '',
            encryptedJson   : ''
        };
    }
}