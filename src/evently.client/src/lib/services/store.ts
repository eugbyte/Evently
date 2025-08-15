import { Store } from "@tanstack/react-store";

export interface StoreState {
	identityUserId: string;
}

export const store = new Store<StoreState>({
	identityUserId: ""
});
