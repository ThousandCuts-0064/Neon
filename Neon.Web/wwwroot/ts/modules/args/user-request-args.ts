interface RequesterRequestArgs {
    readonly requesterKey: string;
    readonly requesterUsername: string;
}

interface ResponderRequestArgs {
    readonly responderKey: string;
    readonly responderUsername: string;
}

export interface UserRequestSentArgs extends RequesterRequestArgs { }
export interface UserRequestAcceptedArgs extends ResponderRequestArgs { }
export interface UserRequestDeclinedArgs extends ResponderRequestArgs { }
export interface UserRequestCanceledArgs extends RequesterRequestArgs { }