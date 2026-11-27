import { Link, useLoaderData } from 'react-router'
import MemberCard from '../members/MemberCard'
import Pagination from '../members/Pagination'
import type { likesLoader } from '../likes/likesLoader'

const tabs = [
  { predicate: 'liked', label: 'I liked' },
  { predicate: 'likedBy', label: 'Liked me' },
] as const

export default function LikesPage() {
  const { items, page, totalPages, predicate, likedIds } = useLoaderData<typeof likesLoader>()

  return (
    <section>
      <h1>Likes</h1>

      <nav className="tabs" aria-label="Likes lists">
        {tabs.map((tab) => {
          const active = tab.predicate === predicate
          return (
            <Link
              key={tab.predicate}
              to={`?predicate=${tab.predicate}`}
              className={active ? 'active' : undefined}
              aria-current={active ? 'page' : undefined}
            >
              {tab.label}
            </Link>
          )
        })}
      </nav>

      {items.length === 0 ? (
        <p className="empty">
          {predicate === 'liked' ? "You haven't liked anyone yet." : 'Nobody has liked you yet.'}
        </p>
      ) : (
        <div className="member-grid">
          {items.map((member) => (
            <MemberCard key={member.id} member={member} liked={likedIds.includes(member.id)} />
          ))}
        </div>
      )}

      <Pagination page={page} totalPages={totalPages} />
    </section>
  )
}