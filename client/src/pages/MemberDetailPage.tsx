import { Link, useLoaderData } from 'react-router'
import type { memberDetailLoader } from '../members/memberDetailLoader'
import { timeAgo } from '../members/timeAgo'
export default function MemberDetailPage() {
  const member = useLoaderData<typeof memberDetailLoader>()
  const mainPhoto = member.photos.find((p) => p.isMain)
  const memberSince = new Date(member.createdAt).toLocaleDateString(undefined, {
    month: 'long',
    year: 'numeric',
  })

  return (
    <article className="member-detail">
      <Link to="/members">← Back to members</Link>

      <header className="member-detail-header">
        <div className="member-avatar" aria-hidden="true">
          {mainPhoto ? (
          <img
            src={mainPhoto.url}
            alt={member.knownAs}
            className="member-avatar member-photo"
          />
        ) : (
          <div className="member-avatar" aria-hidden="true">
            {member.knownAs.charAt(0).toUpperCase()}
          </div>
        )}
        </div>
        <div>
          <h1>
            {member.knownAs}, {member.age}
          </h1>
          <p>
            {member.city}, {member.country} · Member since {memberSince}
          </p>
            <p className="last-active">Active {timeAgo(member.lastActive)}</p>
        </div>
      </header>

      <ProfileSection title="About" text={member.introduction} />
      <ProfileSection title="Looking for" text={member.lookingFor} />
      <ProfileSection title="Interests" text={member.interests} />
         <section>
        <h2>Photos</h2>
        {member.photos.length > 0 ? (
          <div className="photo-gallery">
            {member.photos.map((photo) => (
              <img
                key={photo.id}
                src={photo.url}
                alt={`Photo of ${member.knownAs}`}
                loading="lazy"
              />
            ))}
          </div>
        ) : (
          <p className="empty">No photos yet.</p>
        )}
      </section>
    </article>
  )
}

type ProfileSectionProps = { title: string; text: string | null }

function ProfileSection({ title, text }: ProfileSectionProps) {
  return (
    <section>
      <h2>{title}</h2>
      {text ? <p>{text}</p> : <p className="empty">Not filled in yet.</p>}
    </section>
  )
}