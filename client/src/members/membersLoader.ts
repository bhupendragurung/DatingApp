import type { LoaderFunctionArgs } from "react-router";
import {getMembers, type MemberFilters} from "./api/members";

function toNumber(value:string | null): number | undefined {
    if(!value) return undefined;
    const num = Number(value);
    return Number.isInteger(num) ? num : undefined;
}
function filtersFromUrl(params: URLSearchParams): MemberFilters {
  const gender = params.get('gender')
  const orderBy = params.get('orderBy')
  return {
    page: toNumber(params.get('page')),
    minAge: toNumber(params.get('minAge')),
    maxAge: toNumber(params.get('maxAge')),
    gender: gender === 'male' || gender === 'female' ? gender : undefined,
    orderBy: orderBy === 'newest' || orderBy === 'name' ? orderBy : undefined,
  }
}

export function membersLoader({ request }: LoaderFunctionArgs) {
  const url = new URL(request.url);
  const filters = filtersFromUrl(url.searchParams);
  return getMembers(filters);
}