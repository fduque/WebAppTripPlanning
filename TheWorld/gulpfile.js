/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require("gulp-uglify");
var ngAnnotate = require("gulp-ng-annotate");

gulp.task('minify', function () {
    // place code for your default task here
    return gulp.src("wwwroot/js/*.js") //passando pro gulp onde estao os arquivos javascript
        .pipe(ngAnnotate())//aplicando padrao para anotação necessário para minificar o Angular... pois ele da erros se formos minificar direto sem ajustar os parametros
        .pipe(uglify())//esse comando diz para pegar todos os arquivos Javascript no diretorio acima e inserir no projeto
        //o uglify tem o objetivo de minificar/compactar os arquivos para que fiquem o mais leve possivel
        .pipe(gulp.dest("wwwroot/lib/_app")); //passamos o diretorio que irao ficar todos arquivos compactados de javascript
});