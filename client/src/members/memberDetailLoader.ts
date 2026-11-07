import type { LoaderFunctionArgs } from "react-router";
import { getMember } from "./api/members";
export function memberDetailLoader({ params }: LoaderFunctionArgs) {
     if (!params.id) {
    throw new Response('Member not found', { status: 404, statusText: 'Not Found' })
     }
  return getMember(params.id);
}