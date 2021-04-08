var gulp = require('gulp'); 

gulp.task('copy', function (done) {
    gulp.src('../CD.UmbracoFormsMailchimpWorkflow/App_Plugins/CD.*/**/*.*')
        .pipe(gulp.dest('./App_Plugins'));
    done();
});