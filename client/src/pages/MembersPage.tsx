
import { useLoaderData } from 'react-router'
import MemberCard from '../members/MemberCard'
import type { membersLoader } from '../members/membersLoader'
import MemberFilterForm from '../members/MemberFilterForm'
export default function MembersPage() {
  const { items, totalCount } = useLoaderData<typeof membersLoader>()

  return (
    <section>
      <h1>Members</h1>
      <MemberFilterForm />
      <p>{totalCount} members found</p>
      {items.length === 0 ? (
        <p>No members match these filters.</p>
      ) : (
        <div className="member-grid">
          {items.map((member) => (
            <MemberCard key={member.id} member={member} />
          ))}
        </div>
      )}
    </section>
  )
}