var gulp = require('gulp'); 

gulp.task('copy', function (done) {
    gulp.src('../chrden.Umbraco.Forms.Workflows.Mailchimp/App_Plugins/chrden.*/**/*.*')
        .pipe(gulp.dest('./App_Plugins'));
    done();
});