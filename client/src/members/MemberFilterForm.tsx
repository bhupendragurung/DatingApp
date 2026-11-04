
import { Form, Link, useSearchParams } from 'react-router'

export default function MemberFilterForm() {
  const [searchParams] = useSearchParams()

  return (
    <Form method="get" className="filter-form" key={searchParams.toString()}>
      <label>
        Gender
        <select name="gender" defaultValue={searchParams.get('gender') ?? ''}>
          <option value="">Opposite (default)</option>
          <option value="female">Female</option>
          <option value="male">Male</option>
        </select>
      </label>

      <label>
        Min age
        <input
          type="number"
          name="minAge"
          min={18}
          max={120}
          placeholder="18"
          defaultValue={searchParams.get('minAge') ?? ''}
        />
      </label>

      <label>
        Max age
        <input
          type="number"
          name="maxAge"
          min={18}
          max={120}
          placeholder="100"
          defaultValue={searchParams.get('maxAge') ?? ''}
        />
      </label>

      <label>
        Sort by
        <select name="orderBy" defaultValue={searchParams.get('orderBy') ?? 'newest'}>
          <option value="newest">Newest members</option>
          <option value="name">Name (A–Z)</option>
        </select>
      </label>

      <button type="submit">Apply</button>
      <Link to="/members">Reset</Link>
    </Form>
  )
}