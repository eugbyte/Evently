import { Store } from "@tanstack/react-store";
import { Account } from "~/lib/domains/entities";

export interface StoreState {
	account: Account | null;
}

export const store = new Store<StoreState>({
	account: null
});
