import { PLATFORM } from 'aurelia-framework';
import { I18N } from 'aurelia-i18n';
import { inject } from "aurelia-framework";
import { Router, RouterConfiguration } from 'aurelia-router';
import { HttpClient } from "aurelia-fetch-client";
import { EventAggregator } from 'aurelia-event-aggregator';
import { AuthService } from "../services/auth-service";
import { AuthorizeStep } from "../services/authorization-step";
import { FlashMessageService } from '../services/flash-message-service';

@inject(AuthService, HttpClient, EventAggregator, FlashMessageService, I18N)
export class App {
    authService: AuthService;
    eventAggregator: EventAggregator;
    flashMessageService: FlashMessageService;
    router: Router | undefined;
    i18n: I18N;

    constructor(
        authService: AuthService,
        http: HttpClient,
        eventAggregator: EventAggregator,
        flashMessageService: FlashMessageService,
        i18n: I18N
    ) {
        this.authService = authService;
        this.eventAggregator = eventAggregator;
        this.flashMessageService = flashMessageService;
        http.configure(config => {
            config
                .withBaseUrl('/')
                .withInterceptor(this.authService.tokenInterceptor);
        });
        this.i18n = i18n;
        this.i18n.setLocale('en').then(() => {
            console.log('Locale is ready!', this.i18n.tr('homepagetitle'));
        });
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Hahn Applicaton';
        let step = new AuthorizeStep(this.authService, this.eventAggregator);
        config.addAuthorizeStep(step);
        config.map([{
            route: ['', 'assets'],
            name: 'assets',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../asset/assets'),
            nav: true,
            title: 'Home'
        },
        {
            route: 'addEditAsset/:id',
            name: 'addEditAsset',
            settings: { icon: 'th-list', auth: true },
            moduleId: PLATFORM.moduleName('../asset/add-edit-asset'),
            nav: false,
            title: 'Asset Details'
        },
        {
            route: "login",
            name: "login",
            settings: { icon: 'user' },
            moduleId: PLATFORM.moduleName('../login/login'),
            title: "Login",
            nav: true
        }]);

        this.router = router;
    }
}