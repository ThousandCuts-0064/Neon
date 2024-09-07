abstract class RequesterRequestArgs{
	public constructor(
		public readonly requesterKey: string,
		public readonly requesterUsername: string
	) {}
}

abstract class ResponderRequestArgs{
	public constructor(
		public readonly responderKey: string,
		public readonly responderUsername: string
	) {}
}

export class UserRequestSentArgs extends RequesterRequestArgs {
	public constructor(
		requesterKey: string,
		requesterUsername: string
	) {
		super(requesterKey, requesterUsername);
	}
}

export class UserRequestAcceptedArgs extends ResponderRequestArgs {
	public constructor(
		responderKey: string,
		responderUsername: string
	) {
		super(responderKey, responderUsername);
	}
}

export class UserRequestDeclinedArgs extends ResponderRequestArgs {
	public constructor(
		responderKey: string,
		responderUsername: string
	) {
		super(responderKey, responderUsername);
	}
}

export class UserRequestCanceledArgs extends RequesterRequestArgs {
	public constructor(
		requesterKey: string,
		requesterUsername: string
	) {
		super(requesterKey, requesterUsername);
	}
}