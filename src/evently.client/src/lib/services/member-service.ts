import axios from "axios";
import { Account } from "~/lib/domains/entities";

export async function getMembers(memberId: string): Promise<Account> {
    const response = await axios.get<Account>(`/api/v1/Members/${memberId}`);
    return response.data;
}