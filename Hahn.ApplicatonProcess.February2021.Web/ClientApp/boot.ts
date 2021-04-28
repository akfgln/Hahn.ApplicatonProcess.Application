import 'isomorphic-fetch';
import { I18N, Backend, TCustomAttribute } from 'aurelia-i18n';
import XHR from 'i18next-xhr-backend';
import { Aurelia, PLATFORM } from 'aurelia-framework';
import { HttpClient } from 'aurelia-fetch-client';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap';
declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export function configure(aurelia: Aurelia) {
    aurelia.use.standardConfiguration()
        .plugin(PLATFORM.moduleName('aurelia-validation'))
        .plugin(PLATFORM.moduleName('aurelia-bootstrap-select'))
        .plugin(PLATFORM.moduleName('aurelia-bootstrap-datetimepicker'), config => {
            // extra attributes, with config.extra
            config.extra.iconBase = 'font-awesome';
            config.extra.withDateIcon = true;

            // or even any picker options, with config.options
            config.options.format = 'YYYY-MM-DD';
            config.options.showTodayButton = true;
        })
        .plugin(PLATFORM.moduleName('aurelia-i18n'), (instance) => {
            // register backend plugin
            instance.i18next.use(XHR);

            // adapt options to your needs (see http://i18next.com/docs/options/)
            instance.setup({
                backend: {
                    loadPath: '/locale/{{lng}}/{{ns}}.json',
                },

                lng: 'en',
                attributes: ['t', 'i18n'],
                fallbackLng: 'de',
                debug: false
            });
        });
  
    if (IS_DEV_BUILD) {
        aurelia.use.developmentLogging();
    }
    new HttpClient().configure(config => {
        const baseUrl = document.getElementsByTagName('base')[0].href;
        config.withBaseUrl(baseUrl);
    });

    aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app/components/app/app')));
}
