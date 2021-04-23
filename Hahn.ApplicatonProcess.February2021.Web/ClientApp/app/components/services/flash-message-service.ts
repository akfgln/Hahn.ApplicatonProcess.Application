
import { inject } from 'aurelia-framework'
import { EventAggregator } from 'aurelia-event-aggregator';
import * as toastr from 'toastr';

@inject(EventAggregator)
export class FlashMessageService {
    eventAggregator: EventAggregator;

    constructor(eventAggregator: EventAggregator) {
        this.eventAggregator = eventAggregator;
        this.eventAggregator.subscribe('ewFlashSuccess', this.showSuccess);
        this.eventAggregator.subscribe('ewFlashInfo', this.showInfo);
        this.eventAggregator.subscribe('ewFlashWarning', this.showWarning);
        this.eventAggregator.subscribe('ewFlashError', this.showError);

        toastr.options = {
            positionClass: "toast-top-left",
            showEasing: "swing",
            hideEasing: "linear",
            showMethod: "fadeIn",
            hideMethod: "fadeOut",
            preventDuplicates: true,
            closeButton: true
        }
    }

    showSuccess(message: string) {
        toastr.success(message, null, { preventDuplicates: true, closeButton: true });
    }

    showInfo(message: string) {
        toastr.info(message, null, { preventDuplicates: true, closeButton: true });
    }

    showWarning(message: string) {
        toastr.warning(message, null, { preventDuplicates: true, closeButton: true });
    }

    showError(message: string) {
        toastr.error(message, null, { preventDuplicates: true, closeButton: true });
    }

}