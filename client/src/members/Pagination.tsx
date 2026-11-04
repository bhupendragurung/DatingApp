import {Link, useSearchParams} from 'react-router';
type Props = {page:number; totalPages:number }

export default function Pagination({page, totalPages}:Props){
    const [searchParams] = useSearchParams();
    if(totalPages <= 1) return null;
    function pageLink(target:number){
        const params = new URLSearchParams(searchParams);
        params.set('page', String(target));
        return `?${params.toString()}`;
        
    }
    return (
          <nav className="pagination" aria-label="Member pages">
      {page > 1 ? (
        <Link to={pageLink(page - 1)}>← Previous</Link>
      ) : (
        <span className="disabled">← Previous</span>
      )}

      <span>
        Page {page} of {totalPages}
      </span>

      {page < totalPages ? (
        <Link to={pageLink(page + 1)}>Next →</Link>
      ) : (
        <span className="disabled">Next →</span>
      )}
    </nav>
  )
}