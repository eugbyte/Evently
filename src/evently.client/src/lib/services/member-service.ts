import axios from "axios";
import { Member } from "~/lib/domains/entities";

export async function getMembers(memberId: string): Promise<Member> {
    const response = await axios.get<Member>(`/api/v1/Members/${memberId}`);
    return response.data;
}