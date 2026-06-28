import styles from '@/components/styles/footer.module.css';

export default function Footer() {
    return (
        <div className={styles.footer}>
            <h1>&copy; {new Date().getFullYear()} Rapid Crew & Adrian E. Merli</h1>
        </div>
    )
}