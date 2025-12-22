import Modal from '@/components/shared/modal';
import SignUpForm from '@/components/auth/sign-up-form';

export default function SignUpModal({ show, onClose }: { show: boolean; onClose?: () => void }) {
  return (
    <Modal show={show}>
      <SignUpForm />
    </Modal>
  );
}
