//tripsController


(function () {

    "use-strict";

    //getting the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http)
    {
        var vm = this;
        vm.trips = [];
        vm.newTrip = {};
        vm.errorMessage = "";
        vm.isBusy = true;
        $http.get("/api/trips")
            .then(function (response) {
                //success
                angular.copy(response.data, vm.trips);
            }, function (error) {
                //failure
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function () {
                //inserindo uma variavel para carregar o loading da pagina..
                vm.isBusy = false;
            });

        //vm.addTrip = function () {
        //    //    alert(vm.newTrip.name)
        //    //criando um objeto a partir do parametro name recebido no front..e defininindo o campo data como automatico
        //    vm.trips.push({ name: vm.newTrip.name, created: new Date() });
        //    //para limpar o campo apos preenchido fazemos..
        //    vm.newTrip = {};
        //};

        vm.addTrip = function () {

            vm.errorMessage = "";
            vm.isBusy = true;

                $http.post("/api/trips", vm.newTrip)
                    .then(function (response) {
                        //success
                        vm.trips.push(response.data);
                        //para limpar o campo apos preenchido fazemos..
                        vm.newTrip = {};
                    }, function (error) {
                        //failure
                        vm.errorMessage = "Failed to save new trip: " + error;
                    })
                    .finally(function () {
                        //inserindo uma variavel para carregar o loading da pagina..
                        vm.isBusy = false;
                    });

                };

    }





})();

        ////this represents the object from tripsController
        //var vm = this;
        ////vm.name = "Shawn";

        //vm.trips = [{
        //    name: "US Trip",
        //    created: new Date()
        //}, {
        //    name: "BR Trip",
        //    created: new Date()
        //    }];

        ////recebendo um parametro do html
        //vm.newTrip = {};



