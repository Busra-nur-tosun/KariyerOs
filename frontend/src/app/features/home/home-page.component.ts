import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [RouterLink],
  templateUrl: './home-page.component.html',
})
export class HomePageComponent {
  protected readonly features = [
    {
      icon: 'CV',
      title: 'AI tabanlı dinamik CV optimizasyonu',
      description: 'Her iş ilanı için yapay zeka tarafından gerçek zamanlı düzenlenen, ATS uyumlu CV oluştur.',
    },
    {
      icon: 'SG',
      title: 'Skill gap raporu ve mülakat hazırlığı',
      description: 'Eksik yeteneklerini belirle, sorulara hazırlan ve güçlü yönlerini görünür hale getir.',
    },
    {
      icon: 'MS',
      title: 'Piyasa verileriyle maaş beklentisi normalizasyonu',
      description: 'Gerçekçi maaş beklentileri belirle ve müzakerelere veri destekli hazırlan.',
    },
  ];

  protected readonly matchExamples = [
    { name: 'Ahmet D. - İleri Yazılım Mühendisi', score: '92%' },
    { name: 'Zeynep Y. - Orta Seviye Full Stack', score: '78%' },
    { name: 'Mustafa K. - Junior Yazılım Geliştirici', score: '64%' },
  ];

  protected readonly localDynamics = [
    { title: 'Üniversite', text: 'Üniversite ve bölüm bazlı değerlendirme' },
    { title: 'Sertifikasyon', text: 'Uluslararası ve yerel sertifikalar' },
    { title: 'Askerlik Durumu', text: 'Türkiye iş piyasası dinamiklerine uygun filtreleme' },
    { title: 'Yabancı Dil', text: 'Global roller ve yerel beklentiler için dil seviyesi' },
  ];

  protected readonly steps = [
    {
      number: '1',
      title: 'İş ilanını ekle veya linkini paylaş',
      text: 'Başvurmak istediğin pozisyonun ilanını kopyala veya bağlantısını yapıştır.',
    },
    {
      number: '2',
      title: "AI ile ATS uyumlu CV'ni ve skill gap raporunu al",
      text: 'Dakikalar içinde optimize edilmiş CV ve detaylı skill analizi gör.',
    },
    {
      number: '3',
      title: 'Mülakat hazırlığı ve maaş normalizasyonu',
      text: 'Mülakat sorularına hazır ol ve piyasa verilerine dayalı maaş beklentini netleştir.',
    },
  ];

  protected readonly metrics = [
    { value: '87%', label: 'CV uyum skoru artışı' },
    { value: '3x', label: 'Daha hızlı başvuru hazırlığı' },
    { value: '24/7', label: 'AI destekli kariyer rehberliği' },
  ];

  protected readonly reviews = [
    {
      quote: 'CV’mi her ilana göre yeniden yazmak yerine artık veriyle optimize ediyorum.',
      author: 'Ece K.',
      role: 'Ürün Tasarımcısı',
    },
    {
      quote: 'Skill gap analizi sayesinde hangi alanlara yatırım yapmam gerektiğini net gördüm.',
      author: 'Berk A.',
      role: 'Full Stack Developer',
    },
    {
      quote: 'Mülakat hazırlığı ve maaş benchmarkı aynı yerde olduğu için süreç çok hızlandı.',
      author: 'Selin T.',
      role: 'Product Manager',
    },
  ];

  protected readonly resources = [
    'ATS uyumlu CV rehberi',
    'Mülakat hazırlık notları',
    'Türkiye maaş beklentisi içgörü seti',
  ];
}
