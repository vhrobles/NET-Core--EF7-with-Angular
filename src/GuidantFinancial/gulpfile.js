/// <binding AfterBuild='less, minify' />

var less = require('gulp-less');
var gulp = require('gulp');
var uglify = require('gulp-uglify');

gulp.task('minify', function () {
    return gulp.src('wwwroot/js/*.js').pipe(uglify()).pipe(gulp.dest('wwwroot/lib/_app'));
});

gulp.task('less', function() {
    return gulp.src('wwwroot/css/less/*.less')
        .pipe(less())
        .pipe(gulp.dest('wwwroot/css'));
});