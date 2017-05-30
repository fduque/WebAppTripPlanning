//app-trips

(function () {

    //creating module
    angular.module("app-trips", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {
            $routeProvider.when("/", {
                controller: "tripsController",
                controllerAs: "vm",
                templateUrl: "/tripsView.html"
            });

            $routeProvider.when("/editor/:tripName", {
                controller: "tripsEditorController",
                controllerAs: "vm",
                templateUrl: "/tripsEditorView.html"
            });

            $routeProvider.otherwise({ redirectTo: "/" });
        });
})();